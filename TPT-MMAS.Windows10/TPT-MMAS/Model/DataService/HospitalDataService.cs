using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.API;

namespace TPT_MMAS.Model.DataService
{
    public class HospitalDataService
    {
        public static async Task<List<Admission>> GetAdmissionsAsync(string stationCode)
        {
            try
            {
                string data = await HospitalApi.GetAdmissionsAsync(stationCode);
                List<Admission> admissions = new List<Admission>();

                admissions = JsonConvert.DeserializeObject<List<Admission>>(data);
                return admissions;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
