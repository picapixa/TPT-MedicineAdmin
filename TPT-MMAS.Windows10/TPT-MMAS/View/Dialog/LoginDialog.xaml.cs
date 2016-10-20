using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Model;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View.Dialog
{
    public enum LoginDialogResult
    {
        LoginSuccess,
        Cancel
    }

    public sealed partial class LoginDialog : ContentDialog
    {
        public Personnel _prefilledUser;

        public Personnel AuthenticatedUser { get; set; }

        public LoginDialogResult? Result { get; private set; }

        private bool CanClose { get; set; } = false;


        public LoginDialog(Personnel loggedUser)
        {
            InitializeComponent();

            _prefilledUser = loggedUser;
            Opened += OnLoginDialogOpened;
            Closing += OnLoginDialogClosing;
            Closed += OnLoginDialogClosed;                        
        }

        private void OnLoginDialogClosing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (!CanClose)
                args.Cancel = true;
        }

        private void OnLoginDialogOpened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            tbx_username.Text = _prefilledUser.Username;
        }

        private void OnLoginDialogClosed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (Result == null)
                Result = LoginDialogResult.Cancel;
        }

        private void OnTextBoxChanged(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = (pbx_pw.Password != "" && tbx_username.Text.Trim() != "");
        }

        private async void OnLoginButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                await LoginAsync(args.GetDeferral());
            }
            catch (Exception)
            {
                ResetForm();
            }
        }

        private async Task LoginAsync(ContentDialogButtonClickDeferral deferral = null)
        {
            ContentDialogButtonClickDeferral _deferral;

            pbx_pw.IsEnabled = false;
            tbx_username.IsEnabled = false;
            progress.Visibility = Visibility.Visible;
            IsPrimaryButtonEnabled = false;

            _deferral = deferral;
            try
            {
                Personnel authenticatedUser = await Personnel.AuthenticateAsync(App.ApiSettings, tbx_username.Text, pbx_pw.Password);
                               
                AuthenticatedUser = authenticatedUser;
                Result = LoginDialogResult.LoginSuccess;

                if (_deferral != null)
                    _deferral.Complete();

                CanClose = true;
                Hide();
            }
            catch (ApiException ex)
            {
                MessageDialog md = new MessageDialog(ex.Message);
                await md.ShowAsync();
                
                if (_deferral != null)
                    _deferral.Complete();

                throw ex;
            }
        }

        private void ResetForm()
        {
            //CanClose = false;

            tbx_username.IsEnabled = true;
            pbx_pw.IsEnabled = true;
            pbx_pw.Password = "";
            progress.Visibility = Visibility.Collapsed;
            IsPrimaryButtonEnabled = true;

            //TODO: fire an animation here
        }

        private void OnCancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _prefilledUser = null;
            AuthenticatedUser = null;
            Result = LoginDialogResult.Cancel;

            CanClose = true;
            Hide();
        }
    }
}
