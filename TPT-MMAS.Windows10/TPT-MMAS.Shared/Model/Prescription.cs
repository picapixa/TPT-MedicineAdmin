using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class Prescription
    {
        [JsonProperty("ipm_id")]
        public int ID { get; set; }

        [JsonProperty("ipm_medicine")]
        public MedicineInventory Medicine { get; set; }

        [JsonProperty("ipm_sched")]
        public DateTime Schedule { get; set; }

        [JsonProperty("ipm_amount")]
        public int Amount { get; set; }

        [JsonProperty("ipm_addedon")]
        public DateTime? AddedOn { get; set; }

        [JsonProperty("ipm_hradder")]
        public string AddedBy { get; set; }

        [JsonProperty("ipm_selectedon")]
        public DateTime? SelectedOn { get; set; }

        [JsonProperty("ipm_selector")]
        public string Selector { get; set; }

        [JsonProperty("ipm_loadedon")]
        public DateTime? LoadedOn { get; set; }

        [JsonProperty("ipm_loadedat")]
        public string LoadedAt { get; set; }

        [JsonProperty("ipm_hrloader")]
        public string LoadedBy { get; set; }

        [JsonProperty("ipm_adminedon")]
        public DateTime? AdministeredOn { get; set; }

        [JsonProperty("ipm_hradminer")]
        public string AdministeredBy { get; set; }

    }
}
