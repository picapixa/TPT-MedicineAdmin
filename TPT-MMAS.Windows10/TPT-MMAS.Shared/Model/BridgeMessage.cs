using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class BridgeMessage
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("ipFrom")]
        public string IpFrom { get; set; }

        [JsonProperty("apiSettings")]
        public string ApiSettings { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("param")]
        public string Parameter { get; set; }
    }

}
