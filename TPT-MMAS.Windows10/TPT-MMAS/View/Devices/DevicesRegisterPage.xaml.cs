using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View.Devices
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicesRegisterPage : Page
    {
        public DevicesRegisterPage()
        {
            InitializeComponent();
            Loaded += DevicesRegisterPage_Loaded;
        }

        private void DevicesRegisterPage_Loaded(object sender, RoutedEventArgs e)
        {
            string settingsValue = SettingsHelper.GetLocalSetting("ims_pairedDevice");
            if (settingsValue != null)
            {
                Frame.Navigate(typeof(PatientsPage));
            }

            Frame.BackStack.Clear();
            Shell.UpdateAppViewBackButtonVisibility(Frame);

            if (App.LoggedUser.Role != Role.Admin)
            {
                tb_restricted.Visibility = Visibility.Visible;
                sp_form.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbx_devName.Focus(FocusState.Programmatic);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (Frame.CanGoBack)
            {
                Frame.BackStack.Clear();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            if (await ValidateEntriesAsync(tbx_ipAdd.Text.Trim(), tbx_devName.Text.Trim()))
            {
                IPAddress ipa;
                IPAddress.TryParse(tbx_ipAdd.Text.Trim(), out ipa);

                var imsSvc = new ImsDataService(App.ApiSettings);
                MobileMedAdminSystem system = await imsSvc.RegisterMmasDeviceAsync(tbx_devName.Text, ipa);
                
                var serializedData = JsonConvert.SerializeObject(system);
                SettingsHelper.SetLocalSetting("ims_pairedDevice", serializedData);

                Frame.Navigate(typeof(PatientsPage), system);
            }

        }

        private async Task<bool> ValidateEntriesAsync(string rawIpAddress, string deviceName)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(rawIpAddress, out ipAddress) == false)
            {
                MessageDialog md = new MessageDialog("Please enter a valid IP address.", "Invalid IP Address");
                await md.ShowAsync();

                tbx_ipAdd.Focus(FocusState.Programmatic);
                return false;
            }
            else
                return true;
        }

        private void OnTextBoxInputChanged(object sender, TextChangedEventArgs e)
        {
            btn_submit.IsEnabled = (tbx_devName.Text.Trim() != "" && tbx_ipAdd.Text.Trim() != "");
        }
    }
}
