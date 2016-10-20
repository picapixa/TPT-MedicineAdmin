using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.API
{
    public class ImsApi : WebClient
    {
        private string _baseUri;
        public const string ApiVersion = "v1";
        public const string token = null;

        /// <summary>
        /// Serves as a web client for the IMS API. 
        /// </summary>
        /// <param name="baseUri"></param>
        public ImsApi(string baseUri)
        {
            _baseUri = baseUri + "/api";
        }

        /// <summary>
        /// Gets a JSON-formatted string of machines registered at the database.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRegisteredMachinesInfoAsync()
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "machines");

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        /// <summary>
        /// Updates information about the registered machine and returns the updated data.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<string> PutRegisteredMachineInfoAsync(string deviceName, string ipAddress)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "machines/" + deviceName);

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ipAddress", ipAddress)
            };

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.PUT, uri, token, param);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetAdmissionsAsync(bool inMachineOnly = false)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();

            //add parameters
            if (inMachineOnly)
                param.Add(new KeyValuePair<string, string>("inMachine", "true"));
            else
                param = null;
            
            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> PostAdmissionsAsync(int admissionId)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions");

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("adm_id", admissionId.ToString())
            };

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.POST, uri, token, param);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> PutAdmissionsAsync(int imsId, bool isInMachine)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("inMachine", isInMachine.ToString().ToLower())
            };
            Uri uri = BuildUri(_baseUri, ApiVersion, "admissions/" + imsId, param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.PUT, uri, token);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetMedicineInventoryAsync()
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "inventory");
            
            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> PostMedicineInventoryAsync(string jsonData)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "inventory");

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.POST, uri, token, jsonData);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }


        public async Task<string> PutMedicineInventoryAsync(int medId, string jsonData)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "inventory/" + medId.ToString());

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.PUT, uri, token, jsonData);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetPrescriptionsAsync(int imsId)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "medicines/" + imsId);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> PostPrescriptionAsync(int imsId, string body, int? multiplier = null, int? duration = null)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("aid", imsId.ToString()),
            };
            if (multiplier.HasValue)
                param.Add(new KeyValuePair<string, string>("multiplier", multiplier.Value.ToString()));
            if (duration.HasValue)
                param.Add(new KeyValuePair<string, string>("duration", duration.Value.ToString()));

            Uri uri = BuildUri(_baseUri, ApiVersion, "medicines", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.POST, uri, token, body);
            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> PutPrescriptionsAsync(int imsId, string body, bool isBatch = false)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            if (isBatch)
                param.Add(new KeyValuePair<string, string>("isBatch", "true"));
            else
                param = null;

            Uri uri = BuildUri(_baseUri, ApiVersion, "medicines/" + imsId, param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.PUT, uri, token, body);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> DeletePrescriptionAsync(int id)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "medicines/" + id);
            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.DELETE, uri, token);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> DeletePrescriptionGroupAsync(string body)
        {
            Uri uri = BuildUri(_baseUri, ApiVersion, "medicines/-1");
            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.DELETE, uri, token, body);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }
    }
}
