using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class MobileMedAdminSystem
    {
        [JsonProperty("mmas_id")]
        public int ID { get; set; }

        [JsonProperty("mmas_machine")]
        public string MachineName { get; set; }
        [JsonProperty("mmas_deviceId")]
        public string DeviceID { get; set; }

        [JsonProperty("mmas_ipa")]
        public string IpAddress { get; set; }

        [JsonProperty("mmas_datereg")]
        public DateTime DateRegistered { get; set; }

        [JsonProperty("mmas_lastol")]
        public DateTime? TimeLastOnline { get; set; }
    }
}
