using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TPT_MMAS.Iot.ViewModel;
using TPT_MMAS.Iot.ViewModelMessages;
using TPT_MMAS.Iot.Views;
using TPT_MMAS.Shared.Common;
using TPT_MMAS.Shared.Interface;
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

namespace TPT_MMAS.Iot
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        private Type currentPage;
        private object passedParameter;

        private ShellViewModel VM { get; set; }

        public Shell(Type page, object parameter = null)
        {
            InitializeComponent();
            Loaded += Shell_Loaded;
            Unloaded += Shell_Unloaded;

            currentPage = page;
            passedParameter = parameter;

            VM = DataContext as ShellViewModel;
        }

        private async void HandleMmasAuthenticateMessage(MmasAuthenticateMessage msg)
        {
            await DispatcherHelper.RunAsync(() =>
            {
                App.LoggedUser = msg.User;
                VM.LoggedUser = App.LoggedUser.Username;
                shellFrame.Navigate(typeof(PatientsPage));
            });
        }

        private void HandleLoggingOutMessage(LoggingOutMessage msg)
        {
            VM.LogoutUser(requestedFromDevice: msg.RequestedFromDevice);

            shellFrame.Navigate(typeof(MainPage));

            if (shellFrame.CanGoBack)
                shellFrame.BackStack.Clear();
        }

        /// <summary>
        /// Runs when the shell is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<MmasAuthenticateMessage>(this, HandleMmasAuthenticateMessage);
            Messenger.Default.Register<LoggingOutMessage>(this, HandleLoggingOutMessage);

            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Activate(null);
        }

        /// <summary>
        /// Runs when the shell is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shell_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<MmasAuthenticateMessage>(this, HandleMmasAuthenticateMessage);
            Messenger.Default.Unregister<LoggingOutMessage>(this, HandleLoggingOutMessage);

            var vm = DataContext as INavigable;
            if (vm != null)
                vm.Deactivate(null);
        }


        /// <summary>
        /// Loads the default page when the frame is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shellFrame_Loaded(object sender, RoutedEventArgs e)
        {
            var f = sender as Frame;
            if (f.Content == null)
            {
                f.Navigate(currentPage, passedParameter);
            }
        }
        

        
        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            if (shellFrame.CanGoBack)
            {
                shellFrame.GoBack();
            }
        }
    }
}
