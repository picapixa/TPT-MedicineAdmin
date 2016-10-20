using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.API;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.Shared.Model.DataService
{
    public class HospitalDataService
    {
        private HospitalApi _api;
        private ApiSettings _settings;

        public HospitalDataService(ApiSettings settings)
        {
            _settings = settings;
            _api = new HospitalApi(_settings.HospitalApiBaseUri.ToString());
        }

        public async Task<List<Admission>> GetAdmissionsAsync()
        {
            try
            {
                List<Admission> admissions = new List<Admission>();

                string data = await _api.GetAdmissionsAsync(_settings.StationCode);
                admissions = JsonConvert.DeserializeObject<List<Admission>>(data);

                foreach (var item in admissions)
                {
                    item.LatestFinding = item.Findings.OrderByDescending(f => f.DiagnosedOn).First();
                }

                return admissions;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Admission> GetAdmissionAsync(int id)
        {
            try
            {
                string data = await _api.GetAdmissionAsync(id);
                Admission admission = JsonConvert.DeserializeObject<Admission>(data);

                admission.LatestFinding = admission.Findings.OrderByDescending(f => f.DiagnosedOn).First();

                return admission;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> VerifyAdmissionAsync(int id, string code)
        {
            try
            {
                bool result = await _api.VerifyAdmissionAsync(id, code);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
