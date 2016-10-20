using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Model;
using TPT_MMAS.Shared.Presenters;
using Windows.Networking.Sockets;

namespace TPT_MMAS.Shared.Communications
{
    public class SocketService
    {
        /// <summary>
        /// Sends the action to the device associated with the TcpClient.
        /// </summary>
        /// <param name="client">The TcpClient object with the reference of the device</param>
        /// <param name="apiSettings">The API settings stored in the client application</param>
        /// <param name="action">the name of the action to be executed</param>
        /// <param name="param">Optional parameters related to the action</param>
        /// <returns></returns>
        public static async Task<string> SendToDeviceAsync(TcpClient client, string apiSettings, string action, string param, bool expectResponse = false)
        {
            try
            {
                string body = SerializeMessage(apiSettings, action, param);

                Debug.WriteLine($@"Action {action} sending via TcpClient...", "SocketService");
                string response = await client.SendAsync(body, expectResponse);
                Debug.WriteLine($@"Action {action} sent via TcpClient.", "SocketService");

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the action to the device associated with the TcpClient.
        /// </summary>
        /// <param name="client">The TcpClient object with the reference of the device</param>
        /// <param name="apiSettings">The API settings stored in the client application</param>
        /// <param name="action">the name of the action to be executed</param>
        /// <param name="param">Optional parameters related to the action</param>
        /// <param name="expectResponse">Optionally expect a response to receive from the input stream</param>
        /// <returns></returns>
        public static async Task<string> SendToDeviceAsync(StreamSocket socket, string apiSettings, string action, string param, bool expectResponse = false)
        {
            try
            {
                string response = "";
                string body = SerializeMessage(apiSettings, action, param);

                Debug.WriteLine($@"Action {action} sending via socket...", "SocketService");
                Stream outboundStream = socket.OutputStream.AsStreamForWrite();
                StreamWriter writer = new StreamWriter(outboundStream);
                await writer.WriteLineAsync(body);
                await writer.FlushAsync();
                Debug.WriteLine($@"Action {action} sent via socket.", "SocketService");

                if (expectResponse)
                {
                    Debug.WriteLine("Accessing input stream...", "SocketService");
                    Stream streamIn = socket.InputStream.AsStreamForRead();
                    StreamReader reader = new StreamReader(streamIn);
                    response = await reader.ReadLineAsync();
                    Debug.WriteLine("Response received from input stream.", "SocketService");
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Try to parse the received JSON-formatted bridge message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BridgeMessage TryParseMessage(string message)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<BridgeMessage>(message);

                TimeSpan elapsedTime = DateTime.Now - msg.Timestamp;
                Debug.WriteLine($@"Message received with action '{msg.Action}'. Elapsed time: {elapsedTime.TotalSeconds} s", "SocketService");

                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string SerializeMessage(string apiSettings, string action, string param)
        {
            var dataObj = new BridgeMessage()
            {
                Timestamp = DateTime.Now,
                IpFrom = NetworkPresenter.GetCurrentIpv4Address(),
                ApiSettings = apiSettings,
                Action = action,
                Parameter = param
            };

            string body = JsonConvert.SerializeObject(dataObj, Formatting.None, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return body;
        }

    }
}
