using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware;
using TPT_MMAS.Shared.Interface;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace TPT_MMAS.Iot.ViewModel
{
    public class DebuggingViewModel : ViewModelBase, INavigable
    {
        private ShellViewModel _shellVM;

        public DebuggingViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsScanningEnabled):
                    RfidReader.IsEnabled = IsScanningEnabled.Value;
                    break;
                default:
                    break;
            }
        }

        #region Tray control
        private string _trayStreamContent;

        public string TrayStreamContent
        {
            get { return _trayStreamContent; }
            set { Set(nameof(TrayStreamContent), ref _trayStreamContent, value); }
        }

        private bool _isTrayOpen;

        public bool IsTrayOpen
        {
            get { return _isTrayOpen; }
            set { Set(nameof(IsTrayOpen), ref _isTrayOpen, value); }
        }


        private TrayController _trayController;

        public TrayController TrayController
        {
            get { return _trayController; }
            set { Set(nameof(TrayController), ref _trayController, value); }
        }


        private void LoadTrayController()
        {
            TrayController = TrayController.Instance;
            //TrayController = new TrayController();
            TrayController.PropertyChanged += OnTrayStatusChanged;
            //TrayController.TrayStatusChanged += OnTrayStatusChanged;
            //TrayController.ContainerStateChanged += OnTrayContainerStateChanged;
        }
        

        public async Task EnableLedAsync(int n)
        {
            await TrayController.EnableLEDAsync(n);
        }
        
        private void OnTrayStatusChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsTrayOpen")
                IsTrayOpen = (sender as TrayController).IsTrayOpen;
        }
        #endregion
        
        #region Rfid
        
        private WiegandReader RfidReader { get; set; }

        private string _scannedData;

        public string ScannedData
        {
            get { return _scannedData; }
            set { Set(nameof(ScannedData), ref _scannedData, value); }
        }

        private bool? _isScanningEnabled;

        public bool? IsScanningEnabled
        {
            get { return _isScanningEnabled; }
            set { Set(nameof(IsScanningEnabled), ref _isScanningEnabled, value); }
        }
        
        public void LoadRfidReaderAsync()
        {
            RfidReader = new WiegandReader(23, 24, true);
            RfidReader.RfDataReceived += ReadScannedData;
            RfidReader.IsEnabledChanged += (s, args) =>
            {
                IsScanningEnabled = RfidReader.IsEnabled;
            };
        }

        private async void ReadScannedData(object sender, PropertyChangedEventArgs e)
        {
            string c = RfidReader.RfData.ToString();

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (ulong.Parse(c) <= 0)
                    ScannedData = "try again";
                else
                    ScannedData = c;
            });
        }

        #endregion

        public void Activate(object parameter)
        {
            //tray control
            LoadTrayController();
            
            //back button
            _shellVM = (new ViewModelLocator()).Shell;
            _shellVM.IsBackButtonEnabled = true;
        }

        public void Deactivate(object parameter)
        {
            _shellVM.IsBackButtonEnabled = false;

            if (RfidReader != null)
                RfidReader.Dispose();
        }
    }
}
