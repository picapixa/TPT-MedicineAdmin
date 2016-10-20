using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Common.TPT;

namespace TPT_MMAS.Shared.ViewModel
{
    /// <summary>
    /// An object that inherits the Admission object and adds a provision 
    /// whether the data can be added at IMS or not.
    /// </summary>
    public class AdmissionSuggestion : Admission
    {
        public AdmissionSuggestion()
        {
        }

        public AdmissionSuggestion(Admission a)
        {
            ID = a.ID;
            Patient = a.Patient;
            AttendingPhysicians = a.AttendingPhysicians;
            Findings = a.Findings;
            Room = a.Room;
            Notes = a.Notes;
            StartDate = a.StartDate;
            EndDate = a.EndDate;
            LatestFinding = a.LatestFinding;
        }
        public bool IsAvailableToAdd { get; set; }
    }

    public class PatientsViewModel : BaseViewModel, INavigable, IRefreshable
    {
        private const int MaxPatientNumber = 8;
        private ImsDataService imsSvc;

        public static bool IsOnIot { get; set; }

        #region bindable properties
        
        private ObservableCollection<AdmittedPatient> _patients;

        public ObservableCollection<AdmittedPatient> Patients
        {
            get { return _patients; }
            set { Set(nameof(Patients), ref _patients, value); }
        }
        
        private ObservableCollection<AdmissionSuggestion> _admissionsSuggestions;

        public ObservableCollection<AdmissionSuggestion> AdmissionsSuggestions
        {
            get { return _admissionsSuggestions; }
            set { Set(nameof(AdmissionsSuggestions), ref _admissionsSuggestions, value); }
        }

        private bool _areSlotsNotFull;

        public bool AreSlotsNotFull
        {
            get { return _areSlotsNotFull; }
            set { Set(nameof(AreSlotsNotFull), ref _areSlotsNotFull, value); }
        }

        #endregion

        /// <summary>
        /// Serves as the base view model for the pages related to loading of all selected admitted patients.
        /// </summary>
        public PatientsViewModel()
        {
            IsOnIot = DeviceHelper.IsAppRunningOnIot();
        }

        /// <summary>
        /// Method selector for property changes within the view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Patients):
                    if (Patients != null)
                        AreSlotsNotFull = Patients.Count != MaxPatientNumber;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fires a PropertyChanged event when the collection in Patients property has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Patients_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Patients));
        }
        
        /// <summary>
        /// Retrieves all the admitted patients via the IMS API.
        /// </summary>
        /// <returns></returns>
        public async Task GetAdmittedPatientsAsync()
        {
            try
            {
                IsLoading = true;

                List<AdmittedPatient> patients = await imsSvc.GetAdmittedPatientsAsync(true);

                if (Patients != null)
                {
                    var sortedFreshData = patients.OrderBy(ap => ap.ID);
                    var sortedPatients = Patients.OrderBy(ap => ap.ID);

                    bool areCollectionsEqual = sortedFreshData.SequenceEqual(sortedPatients);

                    if (areCollectionsEqual)
                    {
                        IsLoading = false;
                        return;
                    }
                }

                Patients = new ObservableCollection<AdmittedPatient>(patients);
                Patients.CollectionChanged += Patients_CollectionChanged;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks if all of the admitted patients assigned are ready for preparation.
        /// </summary>
        /// <returns>Boolean value of whether there are no patients that bear the Inactive status</returns>
        public bool CheckIfMedicinesAssignedAreReady()
        {
            return (!(Patients == null) && Patients.Count != 0 && (Patients.All(ap => ap.Remark != PatientAdministrationStatus.Inactive)));
        }
        
        /// <summary>
        /// Retrieves the admissions assigned to the medical unit via the hospital's API.
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<AdmissionSuggestion>> GetAdmissionsFromHospitalAsync()
        {
            var hospSvc = new HospitalDataService(ApiSettings);
            var admissions = await hospSvc.GetAdmissionsAsync();

            var suggestions = new ObservableCollection<AdmissionSuggestion>();
            foreach (var item in admissions)
            {
                AdmissionSuggestion suggestion = new AdmissionSuggestion(item);
                suggestion.IsAvailableToAdd = !Patients.Any(ap => ap.Admission.ID == item.ID);
                suggestions.Add(suggestion);                
            }
            return new ObservableCollection<AdmissionSuggestion>(suggestions.OrderBy(s => s.Room));
        }

        /// <summary>
        /// Adds the selected admission data to the IMS.
        /// </summary>
        /// <param name="admissionObj"></param>
        /// <returns></returns>
        public async Task<AdmittedPatient> AddAdmissionToImsAsync(object admissionObj)
        {
            Admission admission = admissionObj as Admission;
            IsLoading = true;

            try
            {
                AdmittedPatient patient = Patients.First(ap => ap.Admission.ID == admission.ID);
                IsLoading = false;
                return patient;
            }
            catch (InvalidOperationException)
            {
                try
                {
                    var admittedPatient = await imsSvc.AddAdmissionDataAsync(admission);
                    Patients.Add(admittedPatient);
                    IsLoading = false;
                    return admittedPatient;
                }
                catch (Exception)
                {
                    IsLoading = false;
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Fills all of the slots for containers with admitted patients for administration.
        /// </summary>
        /// <returns></returns>
        public async Task FillUpAdmissionSlotsAsync()
        {
            try
            {
                IsLoading = true;
                var suggestions = await GetAdmissionsFromHospitalAsync();
                var unaddedAdmittedPatients = suggestions.Where(sugg => sugg.IsAvailableToAdd == true);
                foreach (var patient in unaddedAdmittedPatients)
                {
                    var admittedPatient = await imsSvc.AddAdmissionDataAsync(patient);
                    if (Patients.Count == MaxPatientNumber)
                        break;
                }
                await RefreshDataAsync();
                IsLoading = false;
            }
            catch (Exception)
            {
                IsLoading = false;
                throw;
            }
        }

        /// <summary>
        /// Disassociates the admitted patient at IMS. Ideal when the patient's medicines
        /// are already administered.
        /// </summary>
        /// <param name="admittedPatientObj"></param>
        /// <returns></returns>
        public async Task RemoveAdmissionFromDeviceAsync(object admittedPatientObj)
        {
            try
            {
                AdmittedPatient patient = admittedPatientObj as AdmittedPatient;
                await imsSvc.RemoveAdmissionDataAsync(patient);
                Patients.Remove(patient as AdmittedPatient);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Disassociates all of the admitted patients at IMS.
        /// </summary>
        /// <returns></returns>
        public async Task ClearAdmittedPatientsAsync()
        {
            try
            {
                var patients = Patients;
                // Why the extra .ToList()? Here's why.
                // http://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute#comment13168776_604843
                foreach (var item in patients.ToList())
                {
                    await RemoveAdmissionFromDeviceAsync(item);
                }
                Patients.Clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Refreshes all the data in the view model.
        /// 
        /// This is an implementation of the IRefresh interface.
        /// </summary>
        /// <returns></returns>
        public async Task RefreshDataAsync()
        {
            await GetAdmittedPatientsAsync();
        }

        /// <summary>
        /// Prepares the data when the page is navigated to. 
        /// 
        /// This is an implementation of the INavigable interface.
        /// </summary>
        /// <param name="parameter">Optional passed parameter</param>
        public virtual async void Activate(object parameter)
        {
            PropertyChanged += OnPropertyChanged;

            imsSvc = new ImsDataService(ApiSettings);
            await GetAdmittedPatientsAsync();
        }

        /// <summary>
        /// Cleans the loaded data when the page is navigated away. 
        /// 
        /// This is an implementation of the INavigable interface.
        /// </summary>
        /// <param name="parameter">Page state from the unloaded event</param>
        public virtual void Deactivate(object parameter)
        {
            PropertyChanged -= OnPropertyChanged;

            if (Patients != null)
            {
                Patients.CollectionChanged -= Patients_CollectionChanged;
                Patients = null;
            }
        }
    }

}
