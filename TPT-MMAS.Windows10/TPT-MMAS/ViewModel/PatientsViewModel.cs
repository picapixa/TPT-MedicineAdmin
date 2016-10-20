using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Model;
using TPT_MMAS.Model.DataService;
using TPT_MMAS.Shared.Interface;

namespace TPT_MMAS.ViewModel
{


    public class PatientsViewModel : BaseViewModel, INavigable
    {

        //TO-DO: SET THE STATION CODE BASED ON DEPLOYMENT CONFIGURATION
        //but for now...
        private string station = "MEDICAL1";

        public static bool IsOnIot { get; set; }


        #region bindable properties
        
        private ObservableCollection<AdmittedPatient> _patients;

        public ObservableCollection<AdmittedPatient> Patients
        {
            get { return _patients; }
            set { Set(nameof(Patients), ref _patients, value); }
        }


        private ObservableCollection<Admission> _admissionsSuggestion;

        public ObservableCollection<Admission> AdmissionsSuggestion
        {
            get { return _admissionsSuggestion; }
            set { Set(nameof(AdmissionsSuggestion), ref _admissionsSuggestion, value); }
        }

        #endregion

        public PatientsViewModel()
        {
            IsOnIot = App.IsRunningOnIOT;
        }

        public async void GetAdmittedPatientsAsync()
        {
            IsLoading = true;

            List<AdmittedPatient> data = await ImsDataService.GetAdmissionsAsync(station, true);
            ObservableCollection<AdmittedPatient> patients = new ObservableCollection<AdmittedPatient>(data);

            if (Patients != null)
            {
                List<AdmittedPatient> sortedFreshData = patients.OrderBy(ap => ap.ID).ToList<AdmittedPatient>();
                List<AdmittedPatient> sortedPatients = Patients.OrderBy(ap => ap.ID).ToList<AdmittedPatient>();

                var areCollectionsEqual = sortedFreshData.SequenceEqual(sortedPatients);
                
                if (areCollectionsEqual)
                {
                    IsLoading = false;
                    return;
                }
            }

            Patients = patients;

            IsLoading = false;
        }
        
        public async void GetAdmissionsFromHospitalAsync()
        {
            if (AdmissionsSuggestion != null)
                return;
            
            var admissions = await HospitalDataService.GetAdmissionsAsync(station);
            AdmissionsSuggestion = new ObservableCollection<Admission>(admissions.OrderBy(adm => adm.Patient.LastName));
        }

        public async void AddAdmissionToImsAsync(object admissionObj)
        {
            Admission admission = admissionObj as Admission;

            bool alreadyExists = Patients.Any<AdmittedPatient>(ap => ap.Admission.ID == admission.ID);
            if (alreadyExists)
                return;

            //register to database
            //--offline? through local cache
            //--online through api
            try
            {
                var admittedPatient = await ImsDataService.PostAdmissionsAsync(admission, station);

                //add to collection
                Patients.Add(admittedPatient);
            }
            catch (Exception)
            {                
                throw;
            }

        }

        #region on view navigated to and from
        public void Activate(object parameter)
        {
            GetAdmittedPatientsAsync();
        }

        public void Deactivate(object parameter)
        {

        }
        #endregion
    }
}
