using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using TPT_MMAS.Shared.ViewModelMessages;

namespace TPT_MMAS.Shared.ViewModel
{
    public class PrescriptionGroup : NotifyPropertyChanged
    {
        private DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { Set(nameof(TimeStamp), ref _timeStamp, value); }
        }

        public ObservableCollection<Prescription> Prescriptions { get; set; }

        public bool IsSelected => Prescriptions.All(p => p.SelectedOn != null);

        public bool IsSelectedOnly => Prescriptions.All(p => (p.SelectedOn != null && p.LoadedOn == null && p.AdministeredOn == null));

        public bool IsLoaded => Prescriptions.All(p => p.LoadedOn != null);
        public bool IsAdministered => Prescriptions.All(p => p.AdministeredOn != null);


    }

    public enum MedicineListFilter
    {
        Scheduled, //future + past & unadministered (30-min window))
        Unadministered, //past and not administered
        Administered, //past and administered
        All
    }

    public class PatientProfileViewModel : BaseViewModel, INavigable, IRefreshable
    {
        protected ImsDataService imsSvc;
        private Personnel loggedUser;

        #region bindable properties

        private ObservableCollection<string> _listFilters;

        public ObservableCollection<string> ListFilters
        {
            get { return _listFilters; }
            set { Set(nameof(ListFilters), ref _listFilters, value); }
        }

        private MedicineListFilter _selectedListFilter;

        public MedicineListFilter SelectedListFilter
        {
            get { return _selectedListFilter; }
            set { Set(nameof(SelectedListFilter), ref _selectedListFilter, value); }
        }


        private AdmittedPatient _admittedPatient;

        public AdmittedPatient AdmittedPatient
        {
            get { return _admittedPatient; }
            set { Set(nameof(AdmittedPatient), ref _admittedPatient, value); }
        }

        private string _nullPrescriptionsContentLabel;

        public string NullPrescriptionsContentLabel
        {
            get { return _nullPrescriptionsContentLabel; }
            set { Set(nameof(NullPrescriptionsContentLabel), ref _nullPrescriptionsContentLabel, value); }
        }
        
        #region prescriptions

        private ObservableCollection<PrescriptionGroup> _prescriptions;

        public ObservableCollection<PrescriptionGroup> Prescriptions
        {
            get { return _prescriptions; }
            set { Set(nameof(Prescriptions), ref _prescriptions, value); }
        }

        private ObservableCollection<PrescriptionGroup> _selectedPrescriptions;

        public ObservableCollection<PrescriptionGroup> SelectedPrescriptions
        {
            get { return _selectedPrescriptions; }
            set { Set(nameof(SelectedPrescriptions), ref _selectedPrescriptions, value); }
        }

        private ObservableCollection<PrescriptionGroup> _allPrescriptions;

        public ObservableCollection<PrescriptionGroup> AllPrescriptions
        {
            get { return _allPrescriptions; }
            set { Set(nameof(AllPrescriptions), ref _allPrescriptions, value); }
        }

        private ObservableCollection<PrescriptionGroup> _scheduledPrescriptions;

        public ObservableCollection<PrescriptionGroup> ScheduledPrescriptions
        {
            get { return _scheduledPrescriptions; }
            set { Set(nameof(ScheduledPrescriptions), ref _scheduledPrescriptions, value); }
        }

        private ObservableCollection<PrescriptionGroup> _administeredPrescriptions;

        public ObservableCollection<PrescriptionGroup> AdministeredPrescriptions
        {
            get { return _administeredPrescriptions; }
            set { Set(nameof(AdministeredPrescriptions), ref _administeredPrescriptions, value); }
        }

        private ObservableCollection<PrescriptionGroup> _unadministeredPrescriptions;

        public ObservableCollection<PrescriptionGroup> UnadministeredPrescriptions
        {
            get { return _unadministeredPrescriptions; }
            set { Set(nameof(UnadministeredPrescriptions), ref _unadministeredPrescriptions, value); }
        }

        #endregion

        private DateTime? _dateLastIntake;

        public DateTime? DateLastIntake
        {
            get { return _dateLastIntake; }
            set { Set(nameof(DateLastIntake), ref _dateLastIntake, value); }
        }
        
        #endregion
        
        /// <summary>
        /// Serves as the base view model for the pages related to a specific patient.
        /// </summary>
        public PatientProfileViewModel()
        {
            var filters = Enum.GetNames(typeof(MedicineListFilter)).ToList();
            ListFilters = new ObservableCollection<string>(filters);
        }

        /// <summary>
        /// Fires a PropertyChanged event when the collection in AllPrescriptions property has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>                        
        private void AllPrescriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(AllPrescriptions));
        }

        /// <summary>
        /// Fires a PropertyChanged event when the collection in SelectedPrescriptions property has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedPrescriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedPrescriptions));
        }

        /// <summary>
        /// Fires a PropertyChanged event when the collection in the current Prescriptions property has changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Prescriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Prescriptions));
        }

        /// <summary>
        /// Method selector for property changes within the view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AllPrescriptions):
                    DividePrescriptions();
                    break;
                case nameof(SelectedListFilter):
                    FilterList(SelectedListFilter);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Divides the prescriptions into their own respective classifications.
        /// </summary>
        private void DividePrescriptions()
        {
            if (ScheduledPrescriptions == null) ScheduledPrescriptions = new ObservableCollection<PrescriptionGroup>();
            if (AdministeredPrescriptions == null) AdministeredPrescriptions = new ObservableCollection<PrescriptionGroup>();
            if (UnadministeredPrescriptions == null) UnadministeredPrescriptions = new ObservableCollection<PrescriptionGroup>();
            if (SelectedPrescriptions == null) SelectedPrescriptions = new ObservableCollection<PrescriptionGroup>();

            // past v. future for scheduled
            List<PrescriptionGroup> recentPast;
            try
            {
                recentPast = AllPrescriptions.Where(grp => (((grp.TimeStamp - DateTime.Now).TotalMinutes > -30) && ((grp.TimeStamp - DateTime.Now).TotalMinutes < 0))).ToList();
            }
            catch (ArgumentNullException)
            {
                recentPast = new List<PrescriptionGroup>();
            }

            List<PrescriptionGroup> future;
            try
            {
                future = AllPrescriptions.Where(grp => (grp.TimeStamp - DateTime.Now).TotalMinutes > 0).ToList();
            }
            catch (ArgumentNullException)
            {
                future = new List<PrescriptionGroup>();
            }
            var oc = recentPast.Union(future);
            ScheduledPrescriptions = new ObservableCollection<PrescriptionGroup>(oc);

            // selected
            var selected = AllPrescriptions.Where(grp => grp.IsSelectedOnly).ToList();
            SelectedPrescriptions = new ObservableCollection<PrescriptionGroup>(selected);
            SelectedPrescriptions.CollectionChanged += SelectedPrescriptions_CollectionChanged;

            // administered v. unadministered
            try
            {
                foreach (PrescriptionGroup group in AllPrescriptions)
                {
                    bool isAdministered = false;
                    foreach (var med in group.Prescriptions)
                    {
                        if (med.AdministeredOn.HasValue)
                        {
                            isAdministered = true;
                            break;
                        }
                    }

                    if (isAdministered)
                        AdministeredPrescriptions.Add(group);
                    else
                        UnadministeredPrescriptions.Add(group);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Filters the Prescriptions property according to the selected filter.
        /// </summary>
        /// <param name="filter">The user's selected filter</param>
        private void FilterList(MedicineListFilter filter)
        {
            switch (filter)
            {
                case MedicineListFilter.All:
                    Prescriptions = AllPrescriptions;
                    NullPrescriptionsContentLabel = "No medicines found. Add one?";
                    break;
                case MedicineListFilter.Scheduled:
                    Prescriptions = ScheduledPrescriptions;
                    NullPrescriptionsContentLabel = "No scheduled medicines found. Add one?";
                    break;
                case MedicineListFilter.Administered:
                    var administered = AdministeredPrescriptions.OrderByDescending(grp => grp.TimeStamp);
                    Prescriptions = new ObservableCollection<PrescriptionGroup>(administered);
                    NullPrescriptionsContentLabel = "No medicines administered yet.";
                    break;
                case MedicineListFilter.Unadministered:
                    var unadministered = UnadministeredPrescriptions.OrderByDescending(grp => grp.TimeStamp);
                    Prescriptions = new ObservableCollection<PrescriptionGroup>(unadministered);
                    NullPrescriptionsContentLabel = "You have not missed any medicines to administer.";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Gets all of the prescriptions from the IMS API and groups them into their schedules.
        /// </summary>
        /// <param name="imsId">ID of admitted patient at the IMS.</param>
        /// <returns>An observable collection of grouped prescriptions.</returns>
        protected async Task<ObservableCollection<PrescriptionGroup>> GetPrescriptionsAsync(int imsId, PatientAdministrationStatus? status = null)
        {
            imsSvc = new ImsDataService(ApiSettings);

            List<Prescription> medicines = await imsSvc.GetPrescriptionsAsync(imsId);

            var grpedMedicines = from meds in medicines
                                 group meds by meds.Schedule into grp
                                 orderby grp.Key
                                 select new { TimeStamp = grp.Key, Items = grp };

            var prescriptionsGrouped = new ObservableCollection<PrescriptionGroup>();

            foreach (var groups in grpedMedicines)
            {
                PrescriptionGroup grp = new PrescriptionGroup();

                grp.TimeStamp = groups.TimeStamp;

                ObservableCollection<Prescription> sortedMedicines = new ObservableCollection<Prescription>();
                foreach (var meds in groups.Items)
                {
                    sortedMedicines.Add(meds);
                }
                grp.Prescriptions = sortedMedicines;

                prescriptionsGrouped.Add(grp);
            }

            if (status.HasValue)
            {
                switch (status.Value)
                {
                    case PatientAdministrationStatus.MedicineSelected:
                        var isSelected = prescriptionsGrouped.Where(grp => grp.IsSelected).ToList();
                        return new ObservableCollection<PrescriptionGroup>(isSelected);
                    case PatientAdministrationStatus.MedicineLoaded:
                        var isLoaded = prescriptionsGrouped.Where(grp => grp.IsLoaded).ToList();
                        return new ObservableCollection<PrescriptionGroup>(isLoaded);
                    case PatientAdministrationStatus.MedicineAdministered:
                        var isAdministered = prescriptionsGrouped.Where(grp => grp.IsAdministered).ToList();
                        return new ObservableCollection<PrescriptionGroup>(isAdministered);
                    default:
                        return prescriptionsGrouped;
                }
            }
            else
                return prescriptionsGrouped;
        }

        /// <summary>
        /// Adds the prescription to the AllPrescriptions property or refreshes it if it's a new group.
        /// </summary>
        /// <param name="item"></param>
        public async void AddPrescriptionToCollection(Prescription item)
        {
            bool isSessionPresent = AllPrescriptions.Any(p => p.TimeStamp == item.Schedule);

            if (isSessionPresent)
            {
                AllPrescriptions.Single(grp => grp.TimeStamp == item.Schedule).Prescriptions.Add(item);
            }
            else
            {
                await RefreshDataAsync();
            }
        }

        /// <summary>
        /// Removes the prescription.
        /// </summary>
        /// <param name="selectedItem"></param>
        public async void DeletePrescription(object selectedItem)
        {
            try
            {
                Prescription item = selectedItem as Prescription;
                await imsSvc.DeletePrescriptionAsync(item);

                var group = AllPrescriptions.Single(grp => grp.TimeStamp == item.Schedule);
                if (group.Prescriptions.Count > 1)
                {
                    AllPrescriptions.Single(grp => grp.TimeStamp == item.Schedule).Prescriptions.Remove(item);
                    Prescriptions.Single(grp => grp.TimeStamp == item.Schedule).Prescriptions.Remove(item);
                }
                else
                {
                    AllPrescriptions.Remove(group);
                    Prescriptions.Remove(group);
                }
            }
            catch (Exception e)
            {
                MessengerInstance.Send(new ExceptionDialogMessage(e));
            }
        }

        /// <summary>
        /// Removes the prescription group.
        /// </summary>
        /// <param name="selectedItems">The prescription group</param>
        public async void DeletePrescriptionGroup(object selectedItems)
        {
            try
            {
                PrescriptionGroup group = selectedItems as PrescriptionGroup;
                var count = group.Prescriptions.Count;

                await imsSvc.DeletePrescriptionGroupAsync(group.Prescriptions);

                AllPrescriptions.Remove(group);
                Prescriptions.Remove(group);
            }
            catch (Exception e)
            {
                MessengerInstance.Send(new ExceptionDialogMessage(e));
            }

        }

        /// <summary>
        /// Picks a prescription group for administration.
        /// </summary>
        /// <param name="user">The currently logged user</param>
        /// <param name="prescriptionGroup">The selected prescription group</param>
        /// <param name="isChecked">Indicates whether the control associated with the action is checked</param>
        public async void SelectPrescriptionGroup(Personnel user, object prescriptionGroup, bool isChecked)
        {
            try
            {
                IsLoading = true;
                PrescriptionGroup group = prescriptionGroup as PrescriptionGroup;

                if (SelectedPrescriptions == null)
                    SelectedPrescriptions = new ObservableCollection<PrescriptionGroup>();

                if (loggedUser == null)
                    loggedUser = user;

                if (isChecked)
                {
                    SelectedPrescriptions.Add(group);
                    foreach (var item in group.Prescriptions)
                    {
                        item.SelectedOn = DateTime.Now;
                        item.Selector = loggedUser.Username;
                    }
                }
                else
                {

                    SelectedPrescriptions.Remove(group);
                    foreach (var item in group.Prescriptions)
                    {
                        item.SelectedOn = null;
                        item.Selector = null;
                    }
                }

                await imsSvc.UpdatePrescriptionsAsync(AdmittedPatient.ID, group.Prescriptions);
               
                IsLoading = false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the time stamp of the last administered prescription.
        /// </summary>
        /// <returns></returns>
        private DateTime? ReturnLastAdministeredPrescription()
        {
            if (AdministeredPrescriptions == null || !AdministeredPrescriptions.Any())
                return null;

            PrescriptionGroup group = AdministeredPrescriptions.OrderBy(grp => grp.Prescriptions[0].AdministeredOn).Last();

            return group.Prescriptions[0].AdministeredOn ?? null;
        }

        /// <summary>
        /// Checks which of the two prescriptions are nearer to the schedule, then sets as the assumed prescription to be administered.
        /// </summary>
        public PrescriptionGroup PickTimelyPrescriptionGroup(IEnumerable<PrescriptionGroup> group)
        {
            PrescriptionGroup lastScheduledPrescription;
            PrescriptionGroup nextScheduledPrescription;

            try
            {
                lastScheduledPrescription = group.Where(grp => (grp.TimeStamp - DateTime.Now).TotalHours < 0).Last();
            }
            catch (InvalidOperationException)
            {
                lastScheduledPrescription = null;
            }

            try
            {
                nextScheduledPrescription = group.Where(grp => (grp.TimeStamp - DateTime.Now).TotalHours >= 0).First();
            }
            catch (InvalidOperationException)
            {
                nextScheduledPrescription = null;
            }

            if (lastScheduledPrescription == null && nextScheduledPrescription == null)
                return null;
            else if (lastScheduledPrescription != null && nextScheduledPrescription == null)
                return lastScheduledPrescription;
            else if (lastScheduledPrescription == null && nextScheduledPrescription != null)
                return nextScheduledPrescription;
            else
            {
                var diffLastSched = (lastScheduledPrescription.TimeStamp - DateTime.Now).TotalMinutes;
                var diffNextSched = (nextScheduledPrescription.TimeStamp - DateTime.Now).TotalMinutes;

                return (Math.Abs(diffLastSched) < Math.Abs(diffNextSched)) ? lastScheduledPrescription : nextScheduledPrescription;
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
            IsLoading = true;
            AllPrescriptions = await GetPrescriptionsAsync(AdmittedPatient.ID);
            DateLastIntake = ReturnLastAdministeredPrescription();
            IsLoading = false;
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

            AdmittedPatient = parameter as AdmittedPatient;
            await RefreshDataAsync();
            AllPrescriptions.CollectionChanged += AllPrescriptions_CollectionChanged;

            SelectedListFilter = MedicineListFilter.Scheduled;
            Prescriptions = ScheduledPrescriptions;
            Prescriptions.CollectionChanged += Prescriptions_CollectionChanged;
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
            Prescriptions.CollectionChanged -= Prescriptions_CollectionChanged;
            SelectedPrescriptions.CollectionChanged -= SelectedPrescriptions_CollectionChanged;
            AllPrescriptions.CollectionChanged -= AllPrescriptions_CollectionChanged;

            SelectedListFilter = MedicineListFilter.All;
            AdmittedPatient = null;
            AllPrescriptions = null;
            SelectedPrescriptions = null;
            AdministeredPrescriptions = null;
            UnadministeredPrescriptions = null;
            ScheduledPrescriptions = null;
        }
    }
}
