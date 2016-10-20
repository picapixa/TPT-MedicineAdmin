using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public enum PatientAdministrationStatus
    {
        Inactive,
        MedicineSelected,
        MedicineLoaded,
        MedicineAdministered
    }

    public class AdmittedPatient
    {
        [JsonProperty("adp_imsid")]
        public int ID { get; set; }

        [JsonProperty("adp_adm")]
        public Admission Admission { get; set; }

        [JsonProperty("adp_remark")]
        public PatientAdministrationStatus Remark { get; set; }

        [JsonProperty("ref_mmas")]
        public MobileMedAdminSystem MMAS { get; set; }
        
    }
}
