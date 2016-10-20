using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Model;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.ViewModel
{
    public class PatientProfileViewModel : BaseViewModel, INavigable
    {

        #region bindable properties
        private AdmittedPatient _admittedPatient;

        public AdmittedPatient AdmittedPatient
        {
            get { return _admittedPatient; }
            set { Set(nameof(AdmittedPatient), ref _admittedPatient, value); }
        }

        #endregion



        public void Activate(object parameter)
        {
            AdmittedPatient = parameter as AdmittedPatient;
            

        }

        public void Deactivate(object parameter)
        {

        }
    }
}
