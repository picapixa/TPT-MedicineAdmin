using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using TPT_MMAS.Iot.Hardware;
using TPT_MMAS.Iot.ViewModelMessages;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Communications;
using TPT_MMAS.Shared.Model;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.System.Threading;

namespace TPT_MMAS.Iot.ViewModel
{
    public enum OperationMode
    {
        Preparation,
        Rounds
    }

    public class ShellViewModel : Shared.ViewModel.ShellViewModel
    {
        private bool _isBackButtonEnabled = false;

        public bool IsBackButtonEnabled
        {
            get { return _isBackButtonEnabled; }
            set { Set(nameof(IsBackButtonEnabled), ref _isBackButtonEnabled, value); }
        }

        private string _loggedUser;

        public string LoggedUser
        {
            get { return _loggedUser; }
            set { Set(nameof(LoggedUser), ref _loggedUser, value); }
        }
        
        private OperationMode _currentOperation = OperationMode.Preparation;

        public OperationMode CurrentOperation
        {
            get { return _currentOperation; }
            set { Set(nameof(CurrentOperation), ref _currentOperation, value); }
        }

        private string _currentOperationText = "PREPARATION MODE";

        public string CurrentOperationText
        {
            get { return _currentOperationText; }
            set { Set(nameof(CurrentOperationText), ref _currentOperationText, value); }
        }
        
        public ShellViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentOperation):
                    CurrentOperationText = (CurrentOperation == OperationMode.Rounds) ?
                        "ROUNDS MODE" : "PREPARATION MODE";
                    break;
                default:
                    break;
            }
        }

        #region clock management
        private DateTime _currentDateTime;

        public DateTime CurrentDateTime
        {
            get { return _currentDateTime; }
            set { Set(nameof(CurrentDateTime), ref _currentDateTime, value); }
        }

        private void LoadClock()
        {
            ThreadPoolTimer clock = null;
            clock = ThreadPoolTimer.CreatePeriodicTimer(_timerTick, TimeSpan.FromMilliseconds(1000));
        }

        private async void _timerTick(ThreadPoolTimer timer)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                CurrentDateTime = DateTime.Now;
            });
        }
        #endregion

        #region TrayController management

        private TrayController _trayController;

        public TrayController TrayController
        {
            get { return _trayController; }
            set { Set(nameof(TrayController), ref _trayController, value); }
        }


        private void AttachTrayController()
        {
            TrayController = TrayController.Instance;     
        }

        #endregion

        #region Sockets
                
        protected override void TcpListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("TcpListener connection received at IoT", "ShellVM.IoT");
        }

        protected override async void TcpListener_ContentReceived(object sender, TcpListenerContentReceivedArgs e)
        {
            try
            {
                await SocketService.SendToDeviceAsync(TcpListener.Socket, null, "confirmReceipt", e.ReceivedContent);

                BridgeMessage message = SocketService.TryParseMessage(e.ReceivedContent);

                if (App.ApiSettings == null)
                {
                    string apiSettings = message.ApiSettings;
                    App.ApiSettings = JsonConvert.DeserializeObject<ApiSettings>(apiSettings);
                }

                if (App.PairedHost == null)
                {
                    HostName hostIp = new HostName(message.IpFrom);
                    App.PairedHost = hostIp;

                    TcpClient = new TcpClient(hostIp, port);
                    await TcpClient.ConnectAsync();
                }

                RunBridgedAction(message.Action, message.Parameter);
            }
            catch (Exception)
            {
                var error = TcpClient.ErrorStatus;
                throw;
            }
        }

        private async void RunBridgedAction(string action, string param)
        {
            switch (action)
            {
                case "authDevice":
                    AuthenticateDevice(account: param);
                    break;
                case "logoutUser":
                    await DispatcherHelper.RunAsync(() => {
                        Messenger.Default.Send(new LoggingOutMessage(isLocallyRequested: false));
                    });
                    break;
                default:
                    return;
            }
        }

        private void AuthenticateDevice(string account)
        {
            Personnel user = JsonConvert.DeserializeObject<Personnel>(account);
            MessengerInstance.Send(new MmasAuthenticateMessage(user));
        }

        public async void LogoutUser(bool requestedFromDevice = true)
        {
            App.LoggedUser = null;
            LoggedUser = "";

            if (requestedFromDevice)
                await SocketService.SendToDeviceAsync(TcpClient, null, "roundsEnded", null);
        }

        #endregion

        public override void Activate(object parameter)
        {
            base.Activate(parameter);
            LoadClock();
            AttachTrayController();
        }

        public override void Deactivate(object parameter)
        {
            base.Deactivate(parameter);
        }
    }
}
