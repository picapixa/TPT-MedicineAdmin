using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.API
{
    public enum HttpVerbs
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class WebClient
    {
        public static async Task<HttpResponseMessage> RunWebClientAsync(HttpVerbs verb, Uri uri, string token, string data = null)
        {
            var client = new HttpClient();
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            

            StringContent param = null;
            if (data != null)
                param = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await RunHttpClientWithMethodAsync(client, verb, uri, param);
            return response;

        }

        public static async Task<HttpResponseMessage> RunWebClientAsync(HttpVerbs verb, Uri uri, string token, IEnumerable<KeyValuePair<string, string>> data)
        {
            var client = new HttpClient();
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            FormUrlEncodedContent param = null;

            if (data != null)
            {
                param = new FormUrlEncodedContent(data);
            }

            HttpResponseMessage response = await RunHttpClientWithMethodAsync(client, verb, uri, param);
            return response;
        }

        public static Uri BuildUri(string baseUri, string apiVer, string path, IEnumerable<KeyValuePair<string, string>> param = null)
        {
            string uri = (apiVer == null) ? $@"{baseUri}/{path}" : $@"{baseUri}/{apiVer}/{path}";

            var builder = new UriBuilder(uri);
            
            if (param != null)
            {
                string query = "";
                foreach (KeyValuePair<string, string> pair in param)
                {
                    if (query != "")
                        query += "&";

                    query += $@"{pair.Key}={pair.Value}";
                }

                builder.Query = query;
            }

            return builder.Uri;
        }
        
        private static async Task<HttpResponseMessage> RunHttpClientWithMethodAsync(HttpClient client, HttpVerbs verb, Uri uri, HttpContent param)
        {
            try
            {
                Debug.WriteLine("HttpClient " + verb.ToString() + ": " + uri.ToString());
                HttpResponseMessage response = null;

                switch (verb)
                {
                    case HttpVerbs.GET:
                        response = await client.GetAsync(uri);
                        break;
                    case HttpVerbs.POST:
                        response = await client.PostAsync(uri, param);
                        break;
                    case HttpVerbs.PUT:
                        response = await client.PutAsync(uri, param);
                        break;
                    case HttpVerbs.DELETE:
                        if (param != null)
                        {
                            HttpRequestMessage req = new HttpRequestMessage()
                            {
                                Content = param,
                                Method = HttpMethod.Delete,
                                RequestUri = uri
                            };
                            response = await client.SendAsync(req);
                        }
                        else
                            response = await client.DeleteAsync(uri);
                        break;
                    default:
                        break;
                }

                return response;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: " + e.InnerException.Message);
                throw e;
            }

            
        }
    }

}
