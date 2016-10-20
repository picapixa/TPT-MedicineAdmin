using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using TPT_MMAS.View.IoT;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using System.Threading.Tasks;

namespace TPT_MMAS
{
    sealed partial class App
    {
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

            string deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;
            IsRunningOnIOT = (deviceFamily == "Windows.IoT") ? true : false;
            //IsRunningOnIOT = true;



            if (IsRunningOnIOT)
            {
                IotShell rootFrame = Window.Current.Content as IotShell;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new IotShell();

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                // Ensure the current window is active
                Window.Current.Activate();

                return;
            }

            Shell clientShell = Window.Current.Content as Shell;

            if (clientShell == null)
            {
                clientShell = new Shell();

                Window.Current.Content = clientShell;

            }

            Window.Current.Activate();

            DispatcherHelper.Initialize();

            //Messenger.Default.Register<NotificationMessageAction<string>>(
            //    this,
            //    HandleNotificationMessage);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // TO-DO: Create URIs that map to a specific functionality.
            // These tpt-mmas:// URIs will be the one where the webserver at the
            // IOT device will redirect to when accomplishing a task coming from
            // the IMS. The webserver should receive RESTful URLs that map to
            // tpt-mmas://. 

            if (!App.IsRunningOnIOT)
            {
                //DON'T DO ANYTHING IF THE APP IS RUNNING ON DESKTOP
            }

            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs e = args as ProtocolActivatedEventArgs;
                //play around with this.
                Debug.WriteLine(e.Uri);
            }
        }

        public static async Task<int> CreateNewWindowAsync<TView>(object param = null)
        {
            var newView = CoreApplication.CreateNewView();
            int newViewId = 0;

            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                var frame = new Frame();
                frame.Navigate(typeof(TView), param);
                Window.Current.Content = frame;

                newViewId = ApplicationView.GetForCurrentView().Id;
                
                ApplicationView.PreferredLaunchViewSize = new Size(320, 400);
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
                ApplicationView.GetForCurrentView().Title = "Add patient to database";
                ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;

                Window.Current.Activate();
            });
            

            return newViewId;
        }

        private static void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            if (!CoreApplication.GetCurrentView().IsMain)
            {
                Window.Current.Close();
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
