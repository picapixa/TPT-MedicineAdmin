using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.API;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.Shared.Model.DataService
{
    public class ImsAdmission
    {
        [JsonProperty("adp_imsid")]
        public int ID { get; set; }

        [JsonProperty("adp_adm")]
        public int AdmissionID { get; set; }

        [JsonProperty("ref_mmas")]
        public int? MmasID { get; set; }

        [JsonProperty("adp_remark")]
        public PatientAdministrationStatus Remark { get; set; }
    }

    public class ImsDataService
    {
        private ImsApi _imsApi;
        private HospitalApi _hospApi;
        private ApiSettings _settings;
        

        public ImsDataService(ApiSettings settings)
        {
            _settings = settings;
            _imsApi = new ImsApi(_settings.ImsApiBaseUri.ToString());
            _hospApi = new HospitalApi(_settings.HospitalApiBaseUri.ToString());
        }

        /// <summary>
        /// Get a list of all the admitted patients.
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="inMachineOnly"></param>
        /// <returns></returns>
        public async Task<List<AdmittedPatient>> GetAdmittedPatientsAsync(bool inMachineOnly = false)
        {
            List<AdmittedPatient> admittedPatients = new List<AdmittedPatient>();
            
            try
            {
                string imsData_raw = await _imsApi.GetAdmissionsAsync(inMachineOnly);
                string mmasData_raw = await _imsApi.GetRegisteredMachinesInfoAsync();
                string hptData_raw = await _hospApi.GetAdmissionsAsync(_settings.StationCode);

                List <ImsAdmission> imsData = JsonConvert.DeserializeObject<List<ImsAdmission>>(imsData_raw);
                List<Admission> hptData = JsonConvert.DeserializeObject<List<Admission>>(hptData_raw);
                List<MobileMedAdminSystem> mmasData = JsonConvert.DeserializeObject<List<MobileMedAdminSystem>>(mmasData_raw);

                foreach (ImsAdmission admission in imsData)
                {
                    var ap = new AdmittedPatient()
                    {
                        ID = admission.ID,
                        Admission = hptData.Where(a => a.ID == admission.AdmissionID).First(),
                        Remark = admission.Remark
                    };

                    if (admission.MmasID != null)
                        ap.MMAS = mmasData.Where(mmas => mmas.ID == admission.MmasID).First();
                    else
                        ap.MMAS = null;

                    ap.Admission.LatestFinding = ap.Admission.Findings.OrderBy(f => f.DiagnosedOn).First();
                                    
                    admittedPatients.Add(ap);
                }

                return admittedPatients;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<AdmittedPatient> AddAdmissionDataAsync(Admission admission)
        {
            try
            {
                string data_raw = await _imsApi.PostAdmissionsAsync(admission.ID);

                ImsAdmission data = JsonConvert.DeserializeObject<ImsAdmission>(data_raw);
                AdmittedPatient patient = ConvertToAdmittedPatient(data, admission);

                return patient;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public async Task RemoveAdmissionDataAsync(AdmittedPatient admittedPatient)
        {
            try
            {
                await _imsApi.PutAdmissionsAsync(admittedPatient.ID, isInMachine: false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MedicineInventory>> GetMedicineInventoryListAsync()
        {
            try
            {
                string data_raw = await _imsApi.GetMedicineInventoryAsync();

                List<MedicineInventory> data = JsonConvert.DeserializeObject<List<MedicineInventory>>(data_raw);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MedicineInventory> AddNewMedicineInventoryAsync(MedicineInventory item)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);

                string receivedItem = await _imsApi.PostMedicineInventoryAsync(data_raw);
                MedicineInventory returnObj = JsonConvert.DeserializeObject<MedicineInventory>(receivedItem);
                return returnObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MedicineInventory> UpdateMedicineInventoryAsync(MedicineInventory item)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);

                string updatedItem = await _imsApi.PutMedicineInventoryAsync(item.ID, data_raw);
                MedicineInventory returnObj = JsonConvert.DeserializeObject<MedicineInventory>(updatedItem);
                return returnObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Prescription>> GetPrescriptionsAsync(int imsId)
        {
            try
            {
                string data_raw = await _imsApi.GetPrescriptionsAsync(imsId);

                List<Prescription> data = JsonConvert.DeserializeObject<List<Prescription>>(data_raw);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<Prescription> AddNewPrescriptionAsync(int imsId, Prescription item)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);

                string response = await _imsApi.PostPrescriptionAsync(imsId, data_raw);
                var returnObj = JsonConvert.DeserializeObject<Prescription>(response);
                return returnObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Prescription>> SetRecurringPrescriptionsAsync(int imsId, Prescription item, int multiplier, int duration)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);

                string response = await _imsApi.PostPrescriptionAsync(imsId, data_raw, multiplier, duration);
                var returnObj = JsonConvert.DeserializeObject<List<Prescription>>(response);
                return returnObj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePrescriptionAsync(int imsId, Prescription item)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);
                string response = await _imsApi.PutPrescriptionsAsync(imsId, data_raw);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdatePrescriptionsAsync(int imsId, IEnumerable<Prescription> items)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(items);
                string response = await _imsApi.PutPrescriptionsAsync(imsId, data_raw, true);

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task DeletePrescriptionAsync(Prescription item)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(item);

                string response = await _imsApi.DeletePrescriptionAsync(item.ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeletePrescriptionGroupAsync(IEnumerable<Prescription> items)
        {
            try
            {
                string data_raw = JsonConvert.SerializeObject(items);
                string response = await _imsApi.DeletePrescriptionGroupAsync(data_raw);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileMedAdminSystem> RegisterMmasDeviceAsync(string deviceName, IPAddress ipAddress)
        {
            try
            {
                string data_raw = await _imsApi.PutRegisteredMachineInfoAsync(deviceName, ipAddress.ToString());

                MobileMedAdminSystem system = JsonConvert.DeserializeObject<MobileMedAdminSystem>(data_raw);
                return system;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static AdmittedPatient ConvertToAdmittedPatient(ImsAdmission iAdm, Admission admission)
        {
            AdmittedPatient ap = new AdmittedPatient()
            {
                ID = iAdm.ID,
                Admission = admission
            };

            return ap;
        }
    }
}
