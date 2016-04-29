using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.API;

namespace TPT_MMAS.Model.DataService
{
    public class ImsAdmission
    {
        [JsonProperty("adp_imsid")]
        public int ID { get; set; }

        [JsonProperty("adp_adm")]
        public int AdmissionID { get; set; }

        [JsonProperty("ref_mmas")]
        public int? MmasID { get; set; }
    }

    public class ImsDataService
    {

        public static async Task<List<AdmittedPatient>> GetAdmissionsAsync(string stationCode, bool inMachineOnly = false)
        {
            List<AdmittedPatient> admittedPatients = new List<AdmittedPatient>();

            try
            {
                string imsData_raw = await ImsApi.GetAdmissionsAsync(inMachineOnly);
                string mmasData_raw = await ImsApi.GetRegisteredMachinesInfoAsync();
                string hptData_raw = await HospitalApi.GetAdmissionsAsync(stationCode);

                List <ImsAdmission> imsData = JsonConvert.DeserializeObject<List<ImsAdmission>>(imsData_raw);
                List<Admission> hptData = JsonConvert.DeserializeObject<List<Admission>>(hptData_raw);
                List<MobileMedAdminMachine> mmasData = JsonConvert.DeserializeObject<List<MobileMedAdminMachine>>(mmasData_raw);

                foreach (ImsAdmission admission in imsData)
                {
                    var ap = new AdmittedPatient()
                    {
                        ID = admission.ID,
                        Admission = hptData.Where(a => a.ID == admission.AdmissionID).First()
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

        public static async Task<AdmittedPatient> PostAdmissionsAsync(Admission admission, string stn_code)
        {
            try
            {
                string data_raw = await ImsApi.PostAdmissionsAsync(admission.ID);

                ImsAdmission data = JsonConvert.DeserializeObject<ImsAdmission>(data_raw);
                AdmittedPatient patient = await ConvertToAdmittedPatientAsync(data, stn_code);

                return patient;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async static Task<AdmittedPatient> ConvertToAdmittedPatientAsync(ImsAdmission iAdm, string stn)
        {
            List<Admission> hptData = await HospitalDataService.GetAdmissionsAsync(stn);

            AdmittedPatient ap = new AdmittedPatient()
            {
                ID = iAdm.ID,
                Admission = hptData.Where(adm => adm.ID == iAdm.AdmissionID).First()
            };

            return ap;
        }
    }
}
