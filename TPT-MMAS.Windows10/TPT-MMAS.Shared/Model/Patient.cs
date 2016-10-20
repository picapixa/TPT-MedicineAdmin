using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Shared.Model
{ 
    public class Patient : Person
    {
        [JsonProperty("pat_id")]
        public new int ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        [JsonProperty("pat_fname")]
        public new string FirstName
        {
            get { return base.FirstName; }
            set { base.FirstName = value; }
        }

        [JsonProperty("pat_mname")]
        public new string MiddleName
        {
            get { return base.MiddleName; }
            set { base.MiddleName = value; }
        }
        [JsonProperty("pat_lname")]
        public new string LastName
        {
            get { return base.LastName; }
            set { base.LastName = value; }
        }

        [JsonProperty("pat_guardian")]
        public string Guardian { get; set; }

        [JsonProperty("pat_contact")]
        public string Contact { get; set; }

    }
}
