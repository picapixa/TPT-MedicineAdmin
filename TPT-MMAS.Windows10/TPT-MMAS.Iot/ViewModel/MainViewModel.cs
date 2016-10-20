using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Presenters;

namespace TPT_MMAS.Iot.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private string _deviceName;

        public string DeviceName
        {
            get { return _deviceName; }
            set { Set(nameof(DeviceName), ref _deviceName, value); }
        }

        private string _deviceIpAddress;

        public string DeviceIpAddress
        {
            get { return _deviceIpAddress; }
            set { Set(nameof(DeviceIpAddress), ref _deviceIpAddress, value); }
        }


        private void SetupDeviceNames()
        {
            DeviceName = DeviceInfoPresenter.GetDeviceName();
            DeviceIpAddress = NetworkPresenter.GetCurrentIpv4Address();
        }

        public void Activate(object parameter)
        {
            SetupDeviceNames();
        }

        public void Deactivate(object parameter)
        {

        }
    }
}
