using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Communications;
using TPT_MMAS.Shared.Interface;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace TPT_MMAS.Shared.ViewModel
{
    public class ShellViewModel : BaseViewModel, INavigable
    {
        public IRefreshable CurrentViewModel { get; set; }

        #region TCP operations
        protected int port = 1000;
        public TcpClient TcpClient { get; set; }
        public TcpListener TcpListener { get; set; }

        /// <summary>
        /// Initializes the TCP listener object.
        /// </summary>
        /// <returns></returns>
        protected async Task InitializeListenerAsync()
        {
            var isOnIot = DeviceHelper.IsAppRunningOnIot();

            try
            {
                TcpListener = new TcpListener(port);
                TcpListener.ConnectionReceived += TcpListener_ConnectionReceived;
                TcpListener.ContentReceived += TcpListener_ContentReceived;
                await TcpListener.StartListeningAsync();

                if (isOnIot)
                    Debug.WriteLine("TcpListener has started listening", "ShellVM on IOT");
                else
                    Debug.WriteLine("TcpListener has started listening", "ShellVM on IMS");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task TcpClientConnectAsync(HostName host, int port)
        {
            TcpClient = new TcpClient(host, port);
            TcpClient.ResponseReceived += TcpClient_ResponseReceived;
            await TcpClient.ConnectAsync();
        }

        protected virtual void TcpClient_ResponseReceived(TcpClient sender, TcpClientResponseReceivedArgs args)
        {
        }

        protected virtual void TcpListener_ContentReceived(object sender, TcpListenerContentReceivedArgs e)
        {
        }

        protected virtual void TcpListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
        }
        #endregion

        /// <summary>
        /// Prepares the data when the shell is loaded.
        /// 
        /// This is an implementation of the INavigable interface.
        /// </summary>
        /// <param name="parameter">Optional passed parameter</param>
        public virtual async void Activate(object parameter)
        {
            if (TcpListener == null)
                await InitializeListenerAsync();
        }

        /// <summary>
        /// Cleans the loaded data when the shell is unloaded.
        /// 
        /// This is an implementation of the INavigable interface.
        /// </summary>
        /// <param name="parameter">Page state from the unloaded event</param>
        public virtual void Deactivate(object parameter)
        {
            TcpListener.ConnectionReceived -= TcpListener_ConnectionReceived;
            TcpListener.ContentReceived -= TcpListener_ContentReceived;

            if (TcpClient != null)
                TcpClient.ResponseReceived -= TcpClient_ResponseReceived;

        }
    }
}
