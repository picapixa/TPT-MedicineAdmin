using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.API
{
    public class HospitalApi : WebClient
    {
        //TO-DO: MAKE AUTH TOKENS

        public const string BaseUri = "http://localhost/tpt-hospital/public/api";
        public const string ApiVersion = "v1";
        public const string token = null;

        public async static Task<string> GetAdmissionsAsync(string stationCode, string remark = "ADMITTED")
        {

            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("station", stationCode),
                new KeyValuePair<string, string>("remark", remark)
            };

            Uri uri = BuildUri(BaseUri, ApiVersion, "admissions", param);

            HttpResponseMessage response = await RunWebClientAsync(HttpVerbs.GET, uri, token, param);
            string raw = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return raw;
        }

    }
}
