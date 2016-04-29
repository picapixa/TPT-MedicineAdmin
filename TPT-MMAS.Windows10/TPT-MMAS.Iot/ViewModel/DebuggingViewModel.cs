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

        private void LoadTrayController()
        {
            TrayController = TrayController.Instance;
            TrayController.TrayStatusChanged += OnTrayStatusChanged;
        }

        private TrayController TrayController { get; set; }

        public async Task EnableLedAsync(int n)
        {
            await TrayController.EnableLEDAsync(n);
        }
        
        private void OnTrayStatusChanged(object sender, PropertyChangedEventArgs e)
        {
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
            string code = await RfidReader.GetRfidDataAsync();
            ScannedData = code;
        }

        private async void ReadScannedData(object sender, PropertyChangedEventArgs e)
        {
            string c = RfidReader.RfData.ToString();

            await ThreadPool.RunAsync((args) => {
                ScannedData = c;
            }, WorkItemPriority.Normal);

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
