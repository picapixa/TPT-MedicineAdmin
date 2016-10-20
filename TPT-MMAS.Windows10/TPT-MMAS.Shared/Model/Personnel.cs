using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.API;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.Shared.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        Staff = 0,
        Nurse = 1,
        Doctor = 2,
        Admin = 3
    }

    public class Personnel : Person
    {
        [JsonProperty("hr_id")]
        public new int ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [JsonProperty("hr_fname")]
        public new string FirstName
        {
            get { return base.FirstName; }
            set { base.FirstName = value; }
        }

        [JsonProperty("hr_mname")]
        public new string MiddleName
        {
            get { return base.MiddleName; }
            set { base.MiddleName = value; }
        }
        [JsonProperty("hr_lname")]
        public new string LastName
        {
            get { return base.LastName; }
            set { base.LastName = value; }
        }

        [JsonProperty("hr_uname")]
        public string Username { get; set; }


        [JsonProperty("hr_title")]
        public string Title { get; set; }

        [JsonProperty("hr_level")]
        private string level { get; set; }

        public Role Role => ParseLevel(level);
        
        public static async Task<Personnel> AuthenticateAsync(ApiSettings settings, string username, string password)
        {
            HospitalApi api = new HospitalApi(settings.HospitalApiBaseUri.ToString());
            HttpResponseMessage resp = await api.AuthenticatePersonnelAsync(username, password);
            string content = await resp.Content.ReadAsStringAsync();
            
            if (resp.IsSuccessStatusCode)
            {
                Personnel data = JsonConvert.DeserializeObject<Personnel>(content);
                return data;
            }
            else
            {
                ApiException error = JsonConvert.DeserializeObject<ApiException>(content);
                error.StatusCode = resp.StatusCode;
                throw error;
            }
        }

        private static Role ParseLevel(string level)
        {
            switch (level)
            {
                case "admin": return Role.Admin;
                case "dr": return Role.Doctor;
                case "rn": return Role.Nurse;
                default: return Role.Staff;
            }
        }
    }
}
