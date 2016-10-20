using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class Finding
    {
        [JsonProperty("find_id")]
        public int ID { get; set; }

        [JsonProperty("ref_dr")]
        public Personnel DiagnosingPhysician { get; set; }

        [JsonProperty("find_diag")]
        public string Diagnosis { get; set; }

        [JsonProperty("find_icd10")]
        public string ICD10Code { get; set; }

        [JsonProperty("find_notes")]
        public string Notes { get; set; }

        [JsonProperty("find_createdon")]
        public DateTime DiagnosedOn { get; set; }
    }
}
