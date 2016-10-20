using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Communications;
using TPT_MMAS.Shared.Interface;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Model.DataService;
using TPT_MMAS.Shared.ViewModel;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace TPT_MMAS.ViewModel
{
    public class ShellViewModel : Shared.ViewModel.ShellViewModel
    {

        #region Tcp-based Bridge

        private bool? _isConnectedToDevice = false;

        public bool? IsConnectedToDevice
        {
            get { return _isConnectedToDevice; }
            set { Set(nameof(IsConnectedToDevice), ref _isConnectedToDevice, value); }
        }

        public async Task ConnectToDeviceAsync(Personnel user)
        {
            string deviceData = SettingsHelper.GetLocalSetting("ims_pairedDevice");
            MobileMedAdminSystem device = JsonConvert.DeserializeObject<MobileMedAdminSystem>(deviceData);
            HostName ip = new HostName(device.IpAddress);

            await TcpClientConnectAsync(ip, port);

            string apiSettings = JsonConvert.SerializeObject(App.ApiSettings);
            string userDetails = JsonConvert.SerializeObject(user);
            
            await SocketService.SendToDeviceAsync(TcpClient, apiSettings, "authDevice", userDetails, expectResponse: true);
        }

        /// <summary>
        /// Sends a command to the device to forcefully log out the user and end the current session.
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectDeviceAsync()
        {
            await SocketService.SendToDeviceAsync(TcpClient, null, "logoutUser", null, expectResponse: true);
        }

        protected override void TcpListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("TcpListener_ConnectionReceived from IMS", "ShellVM.IMS");
        }

        protected override void TcpListener_ContentReceived(object sender, TcpListenerContentReceivedArgs e)
        {
            ProcessReceivedMessage(e.ReceivedContent);
        }

        protected override void TcpClient_ResponseReceived(TcpClient sender, TcpClientResponseReceivedArgs args)
        {
            ProcessReceivedMessage(args.Response);
        }

        private async void ProcessReceivedMessage(string message)
        {
            try
            {
                BridgeMessage response = SocketService.TryParseMessage(message);

                switch (response.Action)
                {
                    case "confirmReceipt":
                        BridgeMessage confirmedMessage = JsonConvert.DeserializeObject<BridgeMessage>(response.Parameter);
                        HandleConfirmedMessage(confirmedMessage);
                        break;
                    case "refreshData":
                        if (CurrentViewModel != null)
                        {
                            await DispatcherHelper.RunAsync(async () => {
                                await CurrentViewModel.RefreshDataAsync();
                            });
                        }
                        break;
                    case "roundsEnded":
                        await DispatcherHelper.RunAsync(() => {
                            IsConnectedToDevice = false;
                        });
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void HandleConfirmedMessage(BridgeMessage confirmedMessage)
        {
            switch (confirmedMessage.Action)
            {
                case "authDevice":
                    await DispatcherHelper.RunAsync(() => { IsConnectedToDevice = true; });
                    break;
                case "logoutUser":
                    await DispatcherHelper.RunAsync(() => { IsConnectedToDevice = false; });
                    break;
                default:
                    break;
            }
        }


        #endregion

        public override void Activate(object parameter)
        {
            base.Activate(parameter);
        }

        public override void Deactivate(object parameter)
        {
            base.Deactivate(parameter);
        }
    }
}
