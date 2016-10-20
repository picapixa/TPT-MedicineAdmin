using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TPT_MMAS.Shared.Communications
{
    public class TcpClient : IDisposable
    {
        public StreamSocket Socket { get; set; }
        public HostName Host { get; set; }
        public int Port { get; set; }

        public SocketErrorStatus? ErrorStatus { get; set; }

        private string _response;

        public string Response
        {
            get { return _response; }
            set { Set(ref _response, value, RaiseResponseReceived); }
        }

        /// <summary>
        /// A client object for the TCP-based inter-device communications protocol.
        /// </summary>
        /// <param name="host">A hostname object that contains the device name/IP address of the device to connect with</param>
        /// <param name="port">The TCP port of the device to connect with</param>
        public TcpClient(HostName host, int port)
        {
            Host = host;
            Port = port;
            Socket = new StreamSocket();
        }

        /// <summary>
        /// Asynchronously connects to the device.
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            try
            {
                await Socket.ConnectAsync(Host, Port.ToString());
            }
            catch (Exception ex)
            {
                ErrorStatus = SocketError.GetStatus(ex.HResult);
                throw;
            }
        }

        /// <summary>
        /// Sends a string to the device.
        /// </summary>
        /// <param name="input">The message to send</param>
        /// <param name="expectResponse">Indicates whether to wait for a response from the device and send it back or not.</param>
        /// <returns>A response string from the device, if expectResponse is enabled.</returns>
        public async Task<string> SendAsync(string input, bool expectResponse = false)
        {
            try
            {
                string response = "";

                Stream outboundStream = Socket.OutputStream.AsStreamForWrite();
                StreamWriter writer = new StreamWriter(outboundStream);
                await writer.WriteLineAsync(input);
                await writer.FlushAsync();

                if (expectResponse)
                    response = await ReadFromStreamAsync();

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reads the input stream for any incoming messages.
        /// </summary>
        /// <returns></returns>
        private async Task<string> ReadFromStreamAsync()
        {
            Task<UInt32> loadAsyncTask;
            uint readBufferLength = 15360; // 1024 x 15

            DataReader dataReader = new DataReader(Socket.InputStream);
            dataReader.InputStreamOptions = InputStreamOptions.Partial;

            loadAsyncTask = dataReader.LoadAsync(readBufferLength).AsTask();
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                var response = dataReader.ReadString(bytesRead);
                Response = response;
                return response;
            }
            else
                return null;
        }

        public TypedEventHandler<TcpClient, TcpClientResponseReceivedArgs> ResponseReceived;

        public void RaiseResponseReceived()
        {
            ResponseReceived?.Invoke(this, new TcpClientResponseReceivedArgs(Response));
        }

        private bool Set<T>(ref T storage, T value, Action raiseHandler, bool checkForEquality = true)
        {
            if (checkForEquality && Equals(storage, value))
                return false;

            storage = value;
            raiseHandler.Invoke();
            return true;
        }

        public void Dispose()
        {
            Socket.Dispose();
            Host = null;
            Port = -1;
            ErrorStatus = null;
        }
    }

    public class TcpClientResponseReceivedArgs : EventArgs
    {
        public string Response { get; set; }
        public TcpClientResponseReceivedArgs(string response)
        {
            Response = response;
        }
    }
}
