using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        Personnel = 0,
        Nurse = 1,
        Doctor = 2,
        Admin = 3
    }

    public class Personnel : Person
    {
        [JsonProperty("hr_id")]
        public new int ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [JsonProperty("hr_fname")]
        public new string FirstName
        {
            get { return base.FirstName; }
            set { base.FirstName = value; }
        }

        [JsonProperty("hr_mname")]
        public new string MiddleName
        {
            get { return base.MiddleName; }
            set { base.MiddleName = value; }
        }
        [JsonProperty("hr_lname")]
        public new string LastName
        {
            get { return base.LastName; }
            set { base.LastName = value; }
        }

        [JsonProperty("hr_title")]
        public string Title { get; set; }

        //[JsonProperty("hr_level")]
        //public Role Role { get; set; }
    }
}
