using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{
    public class MedicineInventory
    {
        [JsonProperty("med_id")]
        public int ID { get; set; }


        [JsonProperty("med_generic")]
        public string GenericName { get; set; }

        [JsonProperty("med_brand")]
        public string BrandName { get; set; }

        [JsonProperty("med_dosage")]
        public string Dosage { get; set; }

        [JsonProperty("med_stocks")]
        public int StocksLeft { get; set; }

        [JsonProperty("med_lastadded")]
        public DateTime TimeLastAdded { get; set; }

        [JsonProperty("med_lastdisp")]
        public DateTime? TimeLastDispensed { get; set; }

        [JsonIgnore]
        public string Name => ConcatenateName(BrandName, GenericName);
        [JsonIgnore]
        public string NameWithDosage => ConcatenateName(BrandName, GenericName, Dosage);

        static string ConcatenateName(string brand, string generic, string dosage = null)
        {
            return (dosage != null) ? $@"{generic} {brand} ({dosage})" : $@"{generic} {brand}";
        }
    }
}
