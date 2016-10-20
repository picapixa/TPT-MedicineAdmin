using System;
using System.Linq;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.ViewModel;
using TPT_MMAS.View.Devices;
using TPT_MMAS.View.Dialog;
using TPT_MMAS.ViewModel;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientsPage : Page
    {
        private bool isAdmissionsCccAttached = false;
        private NavigationHelper navigationHelper;

        private ViewModel.PatientsViewModel VM { get; set; }
        private ViewModel.ShellViewModel ShellViewModel { get; set; }
        

        public PatientsPage()
        {
            InitializeComponent();
            Loaded += PatientsPage_Loaded;
            NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;
            
            VM = DataContext as ViewModel.PatientsViewModel;
            VM.ApiSettings = App.ApiSettings;
            VM.PropertyChanged += VM_PropertyChanged;

            ShellViewModel = new ViewModelLocator().Shell;
            ShellViewModel.PropertyChanged += ShellViewModel_PropertyChanged;
        }

        private void VM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VM.IsLoading):
                    CheckIfAllPatientsHaveMedicines();
                    btn_deviceOptions.IsEnabled = (!VM.IsLoading && App.LoggedUser.Role == Role.Admin);
                    break;
                case nameof(VM.Patients):
                    CheckIfAllPatientsHaveMedicines();
                    break;

                default:
                    break;
            }
        }

        private void ShellViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ShellViewModel.IsConnectedToDevice):
                    mfl_unpairDevice.IsEnabled = !ShellViewModel.IsConnectedToDevice.Value;
                    break;
                default:
                    break;
            }
        }

        private void CheckIfAllPatientsHaveMedicines()
        {
            tbtn_login.IsEnabled = !VM.IsLoading && VM.CheckIfMedicinesAssignedAreReady();
        }

        private void PatientsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (SettingsHelper.GetLocalSetting("ims_pairedDevice") == null)
                Frame.Navigate(typeof(DevicesRegisterPage));
        }

        private void OnPatientItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(PatientProfilePage), e.ClickedItem);
        }

        private void OnPatientGridViewItemRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement el = sender as FrameworkElement;
            FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(el);
            flyout.ShowAt(el);
        }

        #region Patients

        private async void OnFillSlotsClick(object sender, RoutedEventArgs e)
        {
            await VM.FillUpAdmissionSlotsAsync();
        }

        private async void OnRemovePatientClick(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Are you sure you want to remove this patient for medicine administration?");
            md.Commands.Add(new UICommand("Remove", async (args) =>
            {
                try
                {
                    var patient = (e.OriginalSource as FrameworkElement).DataContext;
                    await VM.RemoveAdmissionFromDeviceAsync(patient);
                }
                catch (Exception ex)
                {
                    MessageDialog errorDiag = new MessageDialog(ex.Message);
                    await errorDiag.ShowAsync();
                }
            }));
            md.Commands.Add(new UICommand("Cancel"));

            md.DefaultCommandIndex = 0;
            md.CancelCommandIndex = 1;

            await md.ShowAsync();
        }
        private async void OnClearPatientsClick(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Are you sure you want to clear all patients for medicine administration?");
            md.Commands.Add(new UICommand("Remove", async (args) =>
            {
                try
                {
                    await VM.ClearAdmittedPatientsAsync();
                }
                catch (Exception ex)
                {
                    MessageDialog errorDiag = new MessageDialog(ex.Message);
                    await errorDiag.ShowAsync();
                }
            }));
            md.Commands.Add(new UICommand("Cancel"));

            md.DefaultCommandIndex = 0;
            md.CancelCommandIndex = 1;

            await md.ShowAsync();
        }

        #endregion

        #region Device Management methods

        private async void DeviceConnectToggleButtonClick(object sender, RoutedEventArgs e)
        {
            AdmissionsAutoSuggestBox.IsEnabled = false;
            ToggleButton tbtn = sender as ToggleButton;

            // store the change first before fully implementing it
            var isChecked = tbtn.IsChecked;
            tbtn.IsChecked = !tbtn.IsChecked;
            
            tbtn.IsEnabled = false;

            // if attempting to connect
            if (tbtn.IsChecked == false)
            {
                try
                {
                    LoginDialog loginDialog = new LoginDialog(loggedUser: App.LoggedUser);
                    await loginDialog.ShowAsync();

                    if (loginDialog.Result == LoginDialogResult.LoginSuccess)
                    {
                        VM.IsLoading = true;
                        pr_modalRing.Focus(FocusState.Programmatic);

                        if (ShellViewModel.IsConnectedToDevice.Value == false)
                            await ShellViewModel.ConnectToDeviceAsync(loginDialog.AuthenticatedUser);
                    }
                }
                catch (Exception ex)
                {
                    VM.IsLoading = false;
                    tbtn.IsEnabled = true;
                    AdmissionsAutoSuggestBox.IsEnabled = true;

                    MessageDialog md = new MessageDialog(ex.Message);
                    await md.ShowAsync();
                }
            }
            else
            {
                VM.IsLoading = true;
                await ShellViewModel.DisconnectDeviceAsync();
                tbtn_login.Focus(FocusState.Programmatic);
            }

            VM.IsLoading = false;
            tbtn.IsEnabled = true;
            AdmissionsAutoSuggestBox.IsEnabled = true;
        }

        private async Task LaunchDevicePortalAsync(bool useIpAddress = false)
        {
            string baseUri = (useIpAddress) ? $@"http://{VM.Device.IpAddress}:8080" : $@"http://{VM.Device.MachineName}:8080";

            var success = await Launcher.LaunchUriAsync(new Uri(baseUri));
        }

        #region Device Portal button methods
        
        private void tbtn_login_CheckedChanged(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            tb.Content = (tb.IsChecked.Value == true) ? "Disconnect" : "Connect + Sync";
        }

        private async void OnDevicePortalButtonClick(object sender, RoutedEventArgs e)
        {
            var s = sender as UIElement;

            if (s.GetType() == typeof(MenuFlyoutItem) && (s as MenuFlyoutItem).Name == "mfl_openViaIP")
                await LaunchDevicePortalAsync(true);
            else
                await LaunchDevicePortalAsync();
        }

        private async void OnDevicePortalButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            await LaunchDevicePortalAsync();
        }

        private void OnDevicePortalButtonRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var btn = sender as Button;
            var flyout = FlyoutBase.GetAttachedFlyout(btn);
            flyout.ShowAt(btn);
        }


        private void OnDeviceOptionsButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var flyout = FlyoutBase.GetAttachedFlyout(btn);
            flyout.ShowAt(btn);
        }

        private async void OnChangeDeviceButtonClick(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Are you sure you want to change devices?");
            md.Commands.Add(new UICommand("Continue", (args) =>
            {
                DevicesMainViewModel.RemoveStoredDevice();
                Frame.Navigate(typeof(DevicesRegisterPage));
            }));
            md.Commands.Add(new UICommand("Cancel"));

            md.DefaultCommandIndex = 0;
            md.CancelCommandIndex = 1;

            await md.ShowAsync();
        }
        
        #endregion
        
        #endregion

        #region Admissions Text Box event handlers
        private void AdmissionsTextBoxChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = sender.Text.ToUpper();
                sender.ItemsSource = VM.AdmissionsSuggestions.Where(item =>
                    item.Room.Contains(text) ||
                    item.Patient.FirstName.ToUpper().StartsWith(text) ||
                    item.Patient.LastName.ToUpper().StartsWith(text) ||
                    item.Patient.FullNameAbbreviated.ToUpper().StartsWith(text)
               );
            }
        }

        private async void OnAdmissionsGotFocus(object sender, RoutedEventArgs e)
        {
            VM.AdmissionsSuggestions = await VM.GetAdmissionsFromHospitalAsync();
            AdmissionsAutoSuggestBox.IsSuggestionListOpen = true;

            if (!isAdmissionsCccAttached)
            {
                Popup suggestionsPopup = VisualTreeHelper.GetOpenPopups(Window.Current).First(p => p.Name == "SuggestionsPopup");
                ListView suggestionsList = suggestionsPopup.FindName("SuggestionsList") as ListView;
                suggestionsList.ContainerContentChanging += OnAdmissionsTextBoxSuggestionsListContainerChanging;
                isAdmissionsCccAttached = true;
            }
        }

        private void OnAdmissionsTextBoxSuggestionsListContainerChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer;

            if (itemContainer != null)
            {
                AdmissionSuggestion suggestion = args.Item as AdmissionSuggestion;
                if (!suggestion.IsAvailableToAdd)
                    itemContainer.IsEnabled = false;
            }
        }

        private void OnAdmissionsTextBoxSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as Admission).Patient.FullNameAbbreviated;
        }

        private async void OnAdmissionsTextBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                try
                {
                    object patient = await VM.AddAdmissionToImsAsync(args.ChosenSuggestion);
                    Frame.Navigate(typeof(PatientProfilePage), patient);
                }
                catch (Exception ex)
                {

                    MessageDialog md = new MessageDialog(ex.Message);
                    await md.ShowAsync();
                }
            }
        }
        #endregion

        #region NavigationHelper states

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(e.NavigationParameter);
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Deactivate(e.PageState);
        }
        #endregion

        #region On navigated to and from
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
