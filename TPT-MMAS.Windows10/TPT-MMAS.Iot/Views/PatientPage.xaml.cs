using GalaSoft.MvvmLight.Messaging;
using System;
using TPT_MMAS.Iot.ViewModel;
using TPT_MMAS.Iot.ViewModelMessages;
using TPT_MMAS.Iot.Views.Dialogs;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.Iot.Views
{
    public class Container
    {
        public int ID { get; set; }
        public bool IsMedicineInside { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientPage : Page
    {
        private NavigationHelper navigationHelper;

        private PatientViewModel VM { get; set; }

        public PatientPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += NavigationHelper_LoadState;
            navigationHelper.SaveState += NavigationHelper_SaveState;

            VM = DataContext as PatientViewModel;
            VM.ApiSettings = App.ApiSettings;
        }

        private void HandleTrayClosedMessage(TrayClosedMessage obj)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void HandleErrorDetectedMessage(ErrorDetectedMessage msg)
        {
            ErrorDialog dialog = new ErrorDialog();
            dialog.ErrorMessage = msg.Content;
            dialog.Title = msg.Title;

            await dialog.ShowAsync();
        }

        #region navigationHelper definitions
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Messenger.Default.Register<ErrorDetectedMessage>(this, HandleErrorDetectedMessage);
            Messenger.Default.Register<TrayClosedMessage>(this, HandleTrayClosedMessage);

            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(e.NavigationParameter);
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Messenger.Default.Unregister<ErrorDetectedMessage>(this, HandleErrorDetectedMessage);
            Messenger.Default.Unregister<TrayClosedMessage>(this, HandleTrayClosedMessage);

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
