using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.API
{
    public class ImsApi : WebClient
    {
        public const string BaseUri = "http://localhost/tpt-ims/public/api";
        public const string ApiVersion = "v1";
        public const string token = null;

        public async static Task<string> GetRegisteredMachinesInfoAsync()
        {
            Uri uri = BuildUri(BaseUri, ApiVersion, "machines");

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async static Task<string> GetAdmissionsAsync(bool inMachineOnly = false)
        {
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();

            //add parameters
            if (inMachineOnly)
                param.Add(new KeyValuePair<string, string>("inMachine", "true"));

            //if nothing was added, might as well null it
            if (param.Count == 0)
                param = null;
            
            Uri uri = BuildUri(BaseUri, ApiVersion, "admissions", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }

        public async static Task<string> PostAdmissionsAsync(int admissionId)
        {
            Uri uri = BuildUri(BaseUri, ApiVersion, "admissions");

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("adm_id", admissionId.ToString())
            };

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.POST, uri, token, param);
            string data = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return data;
        }
    }
}
