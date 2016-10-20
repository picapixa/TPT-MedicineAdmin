using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;

namespace TPT_MMAS.Shared.Communications
{
    public class TcpListener : IDisposable
    {        
        public int Port { get; set; }
        public StreamSocket Socket { get; set; }
        public StreamSocketListener Listener { get; set; }
        public Stream InboundStream { get; set; }
        public Stream OutboundStream { get; set; }
        public StreamReader StreamReader { get; set; }
        public StreamWriter StreamWriter { get; set; }

        public SocketErrorStatus? ErrorStatus { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        private string _streamContent;

        public string StreamContent
        {
            get { return _streamContent; }
            set { Set(ref _streamContent, value, RaiseContentReceived, false); }
        }

        /// <summary>
        /// A listener object for the TCP-based inter-device communications protocol.
        /// </summary>
        /// <param name="port">The TCP port of the device to open for connections</param>
        public TcpListener(int port)
        {
            Port = port;

            Listener = new StreamSocketListener();
            
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Opens the specified port for listening to any incoming connections.
        /// </summary>
        /// <returns></returns>
        public async Task StartListeningAsync()
        {
            try
            {
                Listener.ConnectionReceived += Listener_ConnectionReceived;
                Listener.ConnectionReceived += ConnectionReceived;

                await Listener.BindServiceNameAsync(Port.ToString());
            }
            catch (Exception ex)
            {
                ErrorStatus = SocketError.GetStatus(ex.HResult);
                throw;
            }
        }

        /// <summary>
        /// Receives the message sent through the port when a connection to the device is received.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Socket = args.Socket;

            while (true)
            {
                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                await ReadFromStreamAsync();
            }
        }

        /// <summary>
        /// Reads the input stream for any incoming messages and fires the ContentReceived event after.
        /// </summary>
        /// <returns></returns>
        private async Task ReadFromStreamAsync()
        {
            Debug.WriteLine("Accessing input stream...", "TcpListener");

            InboundStream = Socket.InputStream.AsStreamForRead();
            StreamReader = new StreamReader(InboundStream);
            string input = await StreamReader.ReadLineAsync();

            if (input != null)
            {
                Debug.WriteLine("Response received from input stream.", "TcpListener");
                StreamContent = input;
            }
        }

        public TypedEventHandler<StreamSocketListener, StreamSocketListenerConnectionReceivedEventArgs> ConnectionReceived;
        public EventHandler<TcpListenerContentReceivedArgs> ContentReceived;

        private void RaiseContentReceived()
        {
            ContentReceived?.Invoke(this, new TcpListenerContentReceivedArgs(StreamContent));
        }

        private bool Set<T>(ref T storage, T value, Action raiseHandler, bool checkForEquality = true)
        {
            if (checkForEquality && Equals(storage, value))
                return false;

            storage = value;
            raiseHandler.Invoke();
            return true;
        }

        /// <summary>
        /// Closes the connection and cleans all the values related to the object.
        /// </summary>
        public async void Dispose()
        {
            if ((CancellationTokenSource != null) && (CancellationTokenSource.IsCancellationRequested))
                CancellationTokenSource.Cancel();

            if (InboundStream != null)
                InboundStream.Dispose();
            if (OutboundStream != null)
                OutboundStream.Dispose();
            if (StreamReader != null)
                StreamReader.Dispose();
            if (StreamWriter != null)
                StreamWriter.Dispose();

            if (Listener != null)
            {
                await Listener.CancelIOAsync();

                Listener.ConnectionReceived -= Listener_ConnectionReceived;
                Listener.ConnectionReceived -= ConnectionReceived;
                Listener.Dispose();
            }
            Listener = null;

            Port = -1;
            ErrorStatus = null;
            CancellationTokenSource = null;
        }
    }

    public class TcpListenerContentReceivedArgs : EventArgs
    {
        public string ReceivedContent { get; set; }

        public TcpListenerContentReceivedArgs(string content)
        {
            ReceivedContent = content;
        }
    }
}
