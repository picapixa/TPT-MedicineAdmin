using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TPT_MMAS.Shared.Common.TPT;
using TPT_MMAS.Shared.Communications;
using TPT_MMAS.Shared.Model;
using Windows.Networking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TPT_MMAS.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DebugPage : Page
    {
        private string hostIp;
        private int port = 1000;

        private TcpClient tcpClient;

        public DebugPage()
        {
            InitializeComponent();

            string deviceData = SettingsHelper.GetLocalSetting("ims_pairedDevice");
            MobileMedAdminSystem device = JsonConvert.DeserializeObject<MobileMedAdminSystem>(deviceData);
            hostIp = device.IpAddress;

            tb_conndesc.Text = "Connect to remote (" + hostIp + "):";
        }

        private async Task SocketStartAsync()
        {
            try
            {
                tcpClient = new TcpClient(new HostName(hostIp), port);
                await tcpClient.ConnectAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void ConnectButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                btn.IsEnabled = false;

                await SocketStartAsync();

                tbx_input.IsEnabled = true;
                btn_send.IsEnabled = true;

                //btn.IsEnabled = true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void WriteLineToOutput(string line)
        {
            tb_output.Text += line + Environment.NewLine;
        }

        private async void OnSendMessageButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = false;
            tbx_input.IsEnabled = false;

            await tcpClient.SendAsync(tbx_input.Text);

            btn.IsEnabled = true;
            tbx_input.IsEnabled = true;
        }
    }
}
