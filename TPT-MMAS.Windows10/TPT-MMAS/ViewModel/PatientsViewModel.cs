using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Model;
using Windows.Storage;

namespace TPT_MMAS.ViewModel
{
    public class PatientsViewModel : Shared.ViewModel.PatientsViewModel
    {

        #region device management
        private MobileMedAdminSystem _device;

        public MobileMedAdminSystem Device
        {
            get { return _device; }
            set { Set(nameof(Device), ref _device, value); }
        }

        public static bool CheckIfMachineExists()
        {
            string pairedDevice = SettingsHelper.GetLocalSetting("ims_pairedDevice");
            return (pairedDevice != null);
        }

        public static MobileMedAdminSystem GetStoredDevice()
        {
            string pairedDevice = SettingsHelper.GetLocalSetting("ims_pairedDevice");

            if (pairedDevice != null)
            {
                MobileMedAdminSystem system = JsonConvert.DeserializeObject<MobileMedAdminSystem>(pairedDevice);
                return system;
            }
            else
                return null;
        }

        public static bool RemoveStoredDevice()
        {
            try
            {
                SettingsHelper.SetLocalSetting("ims_pairedDevice", null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        
        public override void Activate(object parameter)
        {
            base.Activate(parameter);
            
            if (parameter != null)
                Device = parameter as MobileMedAdminSystem;
            else
                Device = GetStoredDevice();
        }

        public override void Deactivate(object parameter)
        {
            base.Deactivate(parameter);
        }
    }
}
