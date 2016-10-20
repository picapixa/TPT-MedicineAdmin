using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.ViewModel;
using Windows.Storage;

namespace TPT_MMAS.ViewModel
{
    public class DevicesMainViewModel : BaseViewModel, INavigable
    {
        private MobileMedAdminSystem _device;

        public MobileMedAdminSystem Device
        {
            get { return _device; }
            set { Set(nameof(Device), ref _device, value); }
        }

        public static bool CheckIfMachineExists()
        {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string pairedDevice = localSettings.Values["ims_pairedDevice"] as string;
            
            return (pairedDevice != null);
        }

        public static MobileMedAdminSystem GetStoredDevice()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string pairedDevice = localSettings.Values["ims_pairedDevice"] as string;

            MobileMedAdminSystem system = JsonConvert.DeserializeObject<MobileMedAdminSystem>(pairedDevice);
            return system;
        }

        public static bool RemoveStoredDevice()
        {
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["ims_pairedDevice"] = null;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Activate(object parameter)
        {
            if (parameter != null)
                Device = parameter as MobileMedAdminSystem;
            else
                Device = GetStoredDevice();
        }

        public void Deactivate(object parameter)
        {
            Device = null;
        }
    }
}
