using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Model
{
    public class AdmittedPatient
    {
        [JsonProperty("adp_imsid")]
        public int ID { get; set; }

        [JsonProperty("adp_adm")]
        public Admission Admission { get; set; }

        [JsonProperty("ref_mmas")]
        public MobileMedAdminMachine MMAS { get; set; }
    }
}
