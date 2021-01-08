using System;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace GUIforFTP
{
    public class ViewModel
    {
        private int port;
        private FTPClient.FTPClient client;
        private bool connected;

        public event EventHandler<string> HaveMessage;

        public ViewModel()
        {
            port = 49001;
            ServerAddress = "localhost";
            connected = false;
        }

        public string Port
        {
            get => port.ToString();
            set
            {
                if (!int.TryParse(value, out port))
                {
                    HaveMessage?.Invoke(this, "Invalid value of port");
                }
            }
        }

        public string ServerAddress { get; set; }

        public void Connect()
        {
            try
            {
                var tcpClient = new TcpClient(ServerAddress, port);
                client = new FTPClient.FTPClient(tcpClient.GetStream());
                connected = true;
                HaveMessage?.Invoke(this, "Connected");
            }
            catch (SocketException)
            {
                HaveMessage?.Invoke(this, "Connection failed");
            }
        }
    }
}