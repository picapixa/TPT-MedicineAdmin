using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPT_MMAS.Model
{

    public class PatientGroup
    {
        public string GroupName { get; set; }
        public ObservableCollection<AdmittedPatient> AdmittedPatients { get; set; }
    }
}
