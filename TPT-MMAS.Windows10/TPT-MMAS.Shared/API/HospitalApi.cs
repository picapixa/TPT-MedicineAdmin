using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.API
{
    public class HospitalApi : WebClient
    {
        private string _baseUri;
        public const string ApiVersion = "v1";
        public const string token = null;

        /// <summary>
        /// Serves as a web client for the hospital API. 
        /// </summary>
        /// <param name="baseUri"></param>
        public HospitalApi(string baseUri)
        {
            _baseUri = baseUri + "/api";
        }
    
        /// <summary>
        /// Retrieves a JSON string of all admitted patients.
        /// </summary>
        /// <param name="stationCode">The station code where the software is installed</param>
        /// <param name="remark">Expects a string value of either ADMITTED or DISCHARGED</param>
        /// <returns></returns>
        public async Task<string> GetAdmissionsAsync(string stationCode, string remark = "ADMITTED")
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("station", stationCode),
                new KeyValuePair<string, string>("remark", remark)
            };

            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token, param);
            string data = await response.Content.ReadAsStringAsync();
            string raw = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return raw;
        }

        /// <summary>
        /// Gets a specific admitted patient data.
        /// </summary>
        /// <param name="id">The ID of the admission</param>
        /// <returns></returns>
        public async Task<string> GetAdmissionAsync(int id)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions/" + id.ToString());

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string raw = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return raw;
        }

        /// <summary>
        /// Verifies the received key from the RFID if it is associated to the user.
        /// </summary>
        /// <param name="id">The admission ID</param>
        /// <param name="code">The code read from the RFID tag</param>
        /// <returns></returns>
        public async Task<bool> VerifyAdmissionAsync(int id, string code)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("key", code)
            };

            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions/verify/" + id.ToString(), param);
            var response = await RunWebClientAsync(HttpVerbs.POST, uri, token);            
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Authenticates the personnel.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> AuthenticatePersonnelAsync(string username, string password)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("user", username),
                new KeyValuePair<string, string>("key", password)
            };

            Uri uri = BuildUri(_baseUri, ApiVersion, "auth/verify", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.POST, uri, token);
            return response;
        }


    }
}
