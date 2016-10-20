using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Iot.ViewModel;
using TPT_MMAS.Iot.ViewModelMessages;
using TPT_MMAS.Iot.Views.Dialogs;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Control;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.ViewModelMessages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.Iot.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientsPage : Page
    {
        private NavigationHelper navigationHelper;
        private PatientsViewModel VM { get; set; }

        public PatientsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            VM = DataContext as PatientsViewModel;
            VM.ApiSettings = App.ApiSettings;
        }


        private void OnPatientItemClick(object sender, ItemClickEventArgs e)
        {
            var gv = sender as GridView;
            var index = gv.Items.IndexOf(e.ClickedItem);

            var msg = new ListViewBaseNavigationMessage(index, e.ClickedItem);
            Frame.Navigate(typeof(PatientPage), msg);
        }
        private async void OnBeginRoundsModeButtonClick(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;

            if (tb.IsChecked == false)
            {
                ConfirmationDialog confDiag = new ConfirmationDialog(
                    "Ending Rounds Mode will log you out of this machine. Are you sure you want to continue?",
                    "Confirm End of Rounds Mode",
                    "Continue",
                    "Cancel"
                );
                confDiag.PrimaryButtonClick += (s, args) =>
                {
                    VM.ChangeOperationMode(OperationMode.Preparation);
                    (tb.Content as TextBlock).Text = "BEGIN ROUNDS MODE";

                    Messenger.Default.Send(new LoggingOutMessage(isLocallyRequested: true));
                };
                confDiag.SecondaryButtonClick += (s, args) =>
                {
                    tb.IsChecked = true;
                };
                await confDiag.ShowAsync();
            }
            else
            {
                VM.ChangeOperationMode(OperationMode.Rounds);
                (tb.Content as TextBlock).Text = "END ROUNDS MODE";
            }
        }

        #region Navigation helper definitions
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

        #region OnNavigated overrides
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
