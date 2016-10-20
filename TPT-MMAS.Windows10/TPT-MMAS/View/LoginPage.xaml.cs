using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Presenters;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View
{
    public enum LoginPageViewMode
    {
        Progress = 0,
        Setup = 1,
        Login = 2
    }

    internal class LoginPageViewModeVisibilitySwitcher : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            LoginPageViewMode param = (LoginPageViewMode)Enum.Parse(typeof(LoginPageViewMode), parameter as string);

            return ((LoginPageViewMode)value == param) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page, INotifyPropertyChanged
    {
        private LoginPageViewMode _viewMode;
        
        internal LoginPageViewMode ViewMode
        {
            get { return _viewMode; }
            set { Set(nameof(ViewMode), ref _viewMode, value); }
        }


        public LoginPage(LoginPageViewMode mode = LoginPageViewMode.Login)
        {
            InitializeComponent();
            ViewMode = mode;

            if (ViewMode == LoginPageViewMode.Setup)
                FillInExistingSettings();
        }

        #region Setup
        
        /// <summary>
        /// Fills in existing settings on setup.
        /// </summary>
        private void FillInExistingSettings()
        {
            string existingSettings = SettingsHelper.GetLocalSetting("ims_settings");

            if (existingSettings == null)
                return;

            ApiSettings settings = JsonConvert.DeserializeObject<ApiSettings>(existingSettings);
            tbx_hospapi.Text = settings.HospitalApiBaseUri.ToString();
            tbx_imsapi.Text = settings.ImsApiBaseUri.ToString();
            tbx_stncode.Text = settings.StationCode;
        }

        private void OnSetupTextboxChanged(object sender, TextChangedEventArgs e)
        {
            btn_finishsetup.IsEnabled = (tbx_hospapi.Text.Trim() != "" && 
                                        tbx_imsapi.Text.Trim() != "" &&
                                        tbx_stncode.Text.Trim() != "");
        }

        private void OnSetupTextboxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbx = sender as TextBox;
            if (tbx.Text.Trim() == "")
            {
                tbx.Text = "http://" + NetworkPresenter.GetCurrentIpv4Address() + "/" + tbx.Tag + "/public";
            }
        }

        private void OnSetupButtonClick(object sender, RoutedEventArgs e)
        {
            ViewMode = LoginPageViewMode.Progress;

            Uri hospApiUri;
            Uri imsApiUri;

            bool isHospApiUriValid = Uri.TryCreate(tbx_hospapi.Text.Trim(), UriKind.Absolute, out hospApiUri);
            bool isImsApiUriValid = Uri.TryCreate(tbx_imsapi.Text.Trim(), UriKind.Absolute, out imsApiUri);

            if (isHospApiUriValid && isImsApiUriValid)
            {
                ApiSettings settings = new ApiSettings()
                {
                    HospitalApiBaseUri = hospApiUri,
                    ImsApiBaseUri = imsApiUri,
                    StationCode = tbx_stncode.Text.Trim()
                };
                string serializedSettings = JsonConvert.SerializeObject(settings);
                SettingsHelper.SetLocalSetting("ims_settings", serializedSettings);

                App.ApiSettings = settings;

                tbx_un.Text = "";
                pbx_pw.Password = "";
                ViewMode = LoginPageViewMode.Login;
            }
            else
            {
                ViewMode = LoginPageViewMode.Setup;
                FillInExistingSettings();
            }
        }

        #endregion

        #region Login
        private void LoginButtonChangeButtonState(object sender, RoutedEventArgs e)
        {
            btn_login.IsEnabled = (pbx_pw.Password != "" && tbx_un.Text.Trim() != "");
        }
        
        private void OnPasswordBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && btn_login.IsEnabled)
            {
                Login();
            }
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            Login();
        }
        
        private void OnLoginConnectionProblemButtonClick(object sender, RoutedEventArgs e)
        {
            ViewMode = LoginPageViewMode.Setup;
        }

        private async void Login()
        {
            try
            {
                ViewMode = LoginPageViewMode.Progress;
                btn_login.IsEnabled = false;
                await Task.Delay(10);

                Personnel user = await Personnel.AuthenticateAsync(App.ApiSettings, tbx_un.Text, pbx_pw.Password);
                App.LoggedUser = user;

                string pairedDeviceSettings = SettingsHelper.GetLocalSetting("ims_pairedDevice");

                Shell clientShell;
                if (pairedDeviceSettings != null)
                {
                    MobileMedAdminSystem system = JsonConvert.DeserializeObject<MobileMedAdminSystem>(pairedDeviceSettings);
                    clientShell = new Shell(typeof(PatientsPage), system);
                }
                else
                {
                    clientShell = new Shell(typeof(Devices.DevicesRegisterPage));
                }
                Window.Current.Content = clientShell;

                Window.Current.Activate();
            }
            catch (ApiException ex)
            {
                //TODO: If the destination URI is unreachable but internet is present, offer to go to setup
                //hbtn_setup.Visibility = Visibility.Visible;

                MessageDialog md = new MessageDialog(ex.Message, ex.Errors);
                md.Commands.Add(new UICommand("Try again", (command) => {
                    ViewMode = LoginPageViewMode.Login;

                    hbtn_setup.Visibility = Visibility.Collapsed;

                }));
                await md.ShowAsync();
            }
            catch (HttpRequestException ex)
            {
                MessageDialog md = new MessageDialog(ex.InnerException.Data["RestrictedDescription"].ToString(), ex.Message);
                md.Commands.Add(new UICommand("Try again", (command) =>
                {
                    hbtn_setup.Visibility = Visibility.Visible;
                    sp_setupdesc.Text = "Administrator? You may want to check your connection paramters.";

                    ViewMode = LoginPageViewMode.Login;
                }));
                await md.ShowAsync();

            }
        }
        #endregion

        #region INPC implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool Set<T>(string propertyName, ref T storage, T value)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion

    }
}
