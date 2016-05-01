using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Iot.Hardware;
using TPT_MMAS.Shared.Interface;
using System.ComponentModel;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

namespace TPT_MMAS.Iot.ViewModel
{
    public class DebuggingViewModel : ViewModelBase, INavigable
    {

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

        private RfidReader RfidReader { get; set; }

        private string _scannedData;

        public string ScannedData
        {
            get { return _scannedData; }
            set { Set(nameof(ScannedData), ref _scannedData, value); }
        }

        //private bool _isRfScanningEnabled;

        //public bool IsRfScanningEnabled
        //{
        //    get { return _isRfScanningEnabled; }
        //    set { Set(nameof(IsRfScanningEnabled), ref _isRfScanningEnabled, value); }
        //}


        public async void LoadRfidReaderAsync()
        {
            RfidReader = new RfidReader(5, 6);

            // prepare for subsequent scans
            RfidReader.RfDataChanged += ReadScannedData;

            // get first scan

            while (true)
            await RfidReader.GetRfidDataAsync();
        }

        private async void ReadScannedData(object sender, PropertyChangedEventArgs e)
        {
            string c = RfidReader.RfData.ToString();

            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ScannedData = c;
            });

            await RfidReader.GetRfidDataAsync();
        }

        #endregion

        public void Activate(object parameter)
        {
            LoadTrayController();
        }

        public void Deactivate(object parameter)
        {
            RfidReader.RfDataChanged -= ReadScannedData;
        }
    }
}
