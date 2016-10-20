using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware;
using TPT_MMAS.Iot.ViewModelMessages;
using TPT_MMAS.Iot.Views.Dialogs;
using TPT_MMAS.Shared.Communications;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Presenters;
using TPT_MMAS.Shared.ViewModel;
using TPT_MMAS.Shared.ViewModelMessages;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace TPT_MMAS.Iot.ViewModel
{
    public enum Placement
    {
        Correct,
        Incorrect,
        CorrectWithErrors,
        NoChange
    }

    public class PatientViewModel : PatientProfileViewModel
    {
        private string initialPresenceStatus;

        private ShellViewModel _shellVM { get; set; }
        
        private int _patientIndex;

        public int PatientIndex
        {
            get { return _patientIndex; }
            set { Set(nameof(PatientIndex), ref _patientIndex, value); }
        }

        private bool _containerHasItem;

        public bool ContainerHasItem
        {
            get { return _containerHasItem; }
            set { Set(nameof(ContainerHasItem), ref _containerHasItem, value); }
        }


        #region operation levels
        /// <summary>
        /// 0 - loading mode
        /// 1 - preparation mode
        /// 2 - rounds mode - patient verification
        /// 3 - rounds mode - verified patient
        /// </summary>

        private int _mode;

        public int Mode
        {
            get { return _mode; }
            set { Set(nameof(Mode), ref _mode, value); }
        }

        public PatientViewModel()
        {
            PropertyChanged += OnPatientVMPropertyChanged;
        }

        private async void OnPatientVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Mode))
            {
                if (Mode == 1 || Mode == 3)
                {
                    await _shellVM.TrayController.EnableLEDAsync(PatientIndex);
                }
            }
        }
        #endregion
        
        /// <summary>
        /// This is how the patient view will react when something happens at the tray controller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTrayConStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            var trayController = sender as TrayController;
            
            switch (e.PropertyName)
            {
                case "IsTrayOpen":
                    bool isTrayOpen = trayController.IsTrayOpen;
                    if (isTrayOpen == false)
                        HandleOnTrayClosed();
                    break;
                case "HasItem":
                    InspectContainers(trayController.TrayContainers);
                    break;
                case "ContainersRawStatus":
                    if (initialPresenceStatus == null)
                        initialPresenceStatus = _shellVM.TrayController.ContainersRawStatus;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the ContainerHasItem value depending on the current status of the assigned container.
        /// </summary>
        /// <param name="containers"></param>
        private void InspectContainers(IEnumerable<TrayContainer> containers)
        {
            var assignedContainer = TrayController.Instance.TrayContainers.Single(ctr => ctr.ID == PatientIndex);
            ContainerHasItem = assignedContainer.HasItem;
            Debug.WriteLine($@"ContainerHasItem: {ContainerHasItem}", "PatientViewModel");
        }

        /// <summary>
        /// The method to run when the tray is closed.
        /// 
        /// This performs checks related to changes in the controllers both when the tray is opened
        /// and closed, infers the prescription administered based on what group is scheduled the
        /// nearest to current time, and fires the TrayClosedMessage back to the view.
        /// </summary>
        private async void HandleOnTrayClosed()
        {
            bool isDataSent = false;
            
            try
            {
                if (Mode != 2)
                {
                    string finalPresenceStatus = await TrayController.Instance.GetPresenceStatusAsync();

                    Placement placementStatus = EvaluatePresenceStatus(PatientIndex, initialPresenceStatus, finalPresenceStatus);
                    switch (placementStatus)
                    {
                        case Placement.Correct:
                            isDataSent = await UpdatePrescriptionDataAsync(Mode, SelectedPrescriptions, ContainerHasItem);
                            break;
                        case Placement.CorrectWithErrors:
                            isDataSent = await UpdatePrescriptionDataAsync(Mode, SelectedPrescriptions, ContainerHasItem);
                            ErrorDetectedMessage cweMsg = new ErrorDetectedMessage("Conflict detected", "It seems that you have also made changes to other containers. Please place the medicines at the right container.");
                            MessengerInstance.Send(cweMsg);
                            break;
                        case Placement.Incorrect:
                            ErrorDetectedMessage incMsg = new ErrorDetectedMessage("Conflict detected", "It seems that you have placed the prescriptions in the wrong container. Please place the medicines at the right container.");
                            MessengerInstance.Send(incMsg);
                            break;
                        case Placement.NoChange:
                            break;
                    }

                    await SocketService.SendToDeviceAsync(_shellVM.TcpClient, null, "refreshData", null);
                }

                Debug.WriteLine($@"Tray closed, isDataSent: {isDataSent}", "PatientViewModel");
                MessengerInstance.Send(new TrayClosedMessage());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Evaluates the current status of containers.
        /// </summary>
        /// <param name="patientIndex">The current patient index</param>
        /// <param name="previousStatus">The status of containers when the tray is opened</param>
        /// <param name="currentStatus">The status of containers when the tray is closed</param>
        /// <returns></returns>
        private Placement EvaluatePresenceStatus(int patientIndex, string previousStatus, string currentStatus)
        {
            Placement? placement = null;
            string rawPreviousData = previousStatus.Substring(5, 8);
            string rawCurrentData = currentStatus.Substring(5, 8);

            for (int i = 0; i < 8; i++)
            {
                bool isCurrentContainer = (i == patientIndex - 1);
                bool containerStatusHasChanged = rawPreviousData[i] != rawCurrentData[i];

                if (isCurrentContainer && containerStatusHasChanged)
                {
                    placement = (!placement.HasValue) ? Placement.Correct : Placement.CorrectWithErrors;
                }
                else if (!isCurrentContainer && containerStatusHasChanged)
                {
                    if (placement.HasValue && placement.Value == Placement.Correct)
                        placement = Placement.CorrectWithErrors;
                    else
                        placement = Placement.Incorrect;
                }
            }

            return (placement.HasValue) ? placement.Value : Placement.NoChange;
        }

        /// <summary>
        /// Updates the prescription data to the database via the API.
        /// </summary>
        /// <param name="currentMode">Current operation mode of patient view</param>
        /// <param name="groups">The group of medicines administered</param>
        /// <param name="hasItem">Boolean value whether the container has the prescriptions or not</param>
        /// <returns></returns>
        private async Task<bool> UpdatePrescriptionDataAsync(int currentMode, IEnumerable<PrescriptionGroup> groups, bool hasItem)
        {
            int sentCount = 0;
            int grpCount = groups.Count();

            if (currentMode == 1)
            {
                Mode = 0;

                if (hasItem)
                {
                    Debug.WriteLine("Item detected at prep mode");
                    
                    foreach (PrescriptionGroup group in groups)
                    {
                        foreach (var item in group.Prescriptions)
                        {
                            // update count of medicine inventory and timestamp of dispensing
                            if (item.LoadedOn.HasValue == false)
                            {
                                var med = item.Medicine;
                                med.StocksLeft -= item.Amount;
                                med.TimeLastDispensed = DateTime.Now;

                                item.Medicine = await imsSvc.UpdateMedicineInventoryAsync(med);
                            }

                            item.LoadedOn = DateTime.Now;
                            item.LoadedBy = App.LoggedUser.Username;
                            item.LoadedAt = DeviceInfoPresenter.GetDeviceName();
                        }

                        bool isDataSent = await imsSvc.UpdatePrescriptionsAsync(AdmittedPatient.ID, group.Prescriptions);
                        if (isDataSent)
                            sentCount++;
                    }
                    return (grpCount == sentCount);
                }
                else
                    return false;
            }
            else if (currentMode == 3)
            {
                Mode = 0;

                if (!hasItem)
                {
                    Mode = 4;

                    Debug.WriteLine("Vacancy detected at rounds mode");
                    
                    foreach (PrescriptionGroup group in groups)
                    {
                        foreach (var item in group.Prescriptions)
                        {
                            item.AdministeredOn = DateTime.Now;
                            item.AdministeredBy = App.LoggedUser.Username;
                        }

                        bool isDataSent = await imsSvc.UpdatePrescriptionsAsync(AdmittedPatient.ID, group.Prescriptions);
                        if (isDataSent)
                            sentCount++;
                    }
                    return (grpCount == sentCount);
                }
                else
                    return false;
            }
            else
                return false;          
        }

        #region Rfid

        private WiegandReader RfidReader { get; set; }

        private void InitializeRfidReader()
        {
            RfidReader = new WiegandReader(23, 24);
            RfidReader.RfDataReceived += OnRfidDataReceived;
        }

        private async void OnRfidDataReceived(object sender, PropertyChangedEventArgs e)
        {
            string data = RfidReader.RfData.ToString();

            if (RfidReader.IsEnabled == false)
                return;
                       
            bool isVerified = false;
            RfidReader.IsEnabled = false;

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                Mode = 0;

                if (data == "0")
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Mode = -1;
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    Mode = 0;
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Mode = 2;
                    RfidReader.IsEnabled = true;
                    return;
                }

                isVerified = await AdmittedPatient.Admission.VerifyAdmittedPatientAsync(App.ApiSettings, data);

                if (isVerified)
                {
                    Mode = 3;
                }
                else
                {
                    Mode = -1;

                    await Task.Delay(TimeSpan.FromSeconds(2));

                    Mode = 0;

                    await Task.Delay(TimeSpan.FromSeconds(1));

                    Mode = 2;
                    RfidReader.IsEnabled = true;
                }
            });
        }

        #endregion

        private int SetModeSettings()
        {
            int mode = 0;
            if (App.PluggedDevice == Device.Prototype)
            {
                mode = (_shellVM.CurrentOperation == OperationMode.Preparation) ? 1 : 2;
            }
            else
            {
                mode = (_shellVM.CurrentOperation == OperationMode.Preparation) ? 1 : 3;
            }
            return mode;
        }
        
        public override async void Activate(object parameter)
        {
            AdmittedPatient = ((parameter as ListViewBaseNavigationMessage).PassedItem as AdmittedPatient);
            imsSvc = new Shared.Model.DataService.ImsDataService(App.ApiSettings);

            _shellVM = (new ViewModelLocator()).Shell;
            _shellVM.TrayController.PropertyChanged += OnTrayConStatusChanged;

            PatientIndex = (parameter as ListViewBaseNavigationMessage).Index + 1;
            Mode = SetModeSettings();
            if (Mode == 2)
            {
                _shellVM.IsBackButtonEnabled = true;

                if (App.PluggedDevice == Device.Prototype)
                    InitializeRfidReader();
            }

            SelectedPrescriptions = await GetPrescriptionsAsync(AdmittedPatient.ID, PatientAdministrationStatus.MedicineSelected);
        }

        public override void Deactivate(object parameter)
        {
            _shellVM.IsBackButtonEnabled = false;
            _shellVM.TrayController.PropertyChanged -= OnTrayConStatusChanged;
            PatientIndex = 0;
            Mode = 0;
            initialPresenceStatus = null;
            ContainerHasItem = false;

            SelectedPrescriptions = null;

            if (RfidReader != null)
                RfidReader.Dispose();
        }
    }
}
