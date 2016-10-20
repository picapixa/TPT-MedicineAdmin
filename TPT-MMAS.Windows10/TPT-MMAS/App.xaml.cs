using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System.Diagnostics;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Common.TPT;
using Newtonsoft.Json;

namespace TPT_MMAS
{
    sealed partial class App
    {
        public static Personnel LoggedUser { get; set; }

        public static ApiSettings ApiSettings { get; set; }

        public static bool IsRunningOnIOT { get; private set; } = false;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

            //#if DEBUG
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                DebugSettings.EnableFrameRateCounter = true;
            //            }
            //#endif

            Shell clientShell = Window.Current.Content as Shell;
            
            if (clientShell == null)
            {                
                string settings = SettingsHelper.GetLocalSetting("ims_settings");

                View.LoginPage loginPage;
                if (settings != null)
                {
                    ApiSettings = JsonConvert.DeserializeObject<ApiSettings>(settings);
                    loginPage = new View.LoginPage(View.LoginPageViewMode.Login);
                }
                else
                {
                    loginPage = new View.LoginPage(View.LoginPageViewMode.Setup);
                }
                Window.Current.Content = loginPage;
            }

            Window.Current.Activate();

            DispatcherHelper.Initialize();

            //Messenger.Default.Register<NotificationMessageAction<string>>(
            //    this,
            //    HandleNotificationMessage);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {

            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs e = args as ProtocolActivatedEventArgs;
                //play around with this.
                Debug.WriteLine(e.Uri);
            }
        }

        private void HandleNotificationMessage(NotificationMessageAction<string> message)
        {
            message.Execute("Success (from App.xaml.cs)!");
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
       
    }
}
