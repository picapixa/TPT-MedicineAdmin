using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Model
{
    public class Admission
    {
        [JsonProperty("adm_id")]
        public int ID { get; set; }

        [JsonProperty("adm_patient")]
        public Patient Patient { get; set; }

        [JsonProperty("adm_physicians")]
        public List<Personnel> AttendingPhysicians { get; set; }

        [JsonProperty("adm_findings")]
        public List<Finding> Findings { get; set; }

        [JsonProperty("adm_room")]
        public string Room { get; set; }

        public string LabeledRoom => $@"Room {Room}";

        [JsonProperty("adm_notes")]
        public string Notes { get; set; }

        [JsonProperty("adm_datestart")]
        public DateTime StartDate { get; set; }

        [JsonProperty("adm_dateend")]
        public DateTime? EndDate { get; set; }

        public Finding LatestFinding { get; set; }

    }
}
