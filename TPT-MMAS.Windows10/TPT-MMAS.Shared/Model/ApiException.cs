using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class ApiException : Exception
    {
        public string Errors { get; set; }
        public string Exception { get; set; }

        [JsonProperty("message")]
        public new string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ApiException()
        {
        }
    }
}
