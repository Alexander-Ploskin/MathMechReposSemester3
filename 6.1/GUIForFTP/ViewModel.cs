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
            RootFolder = "../../../../";
            CurrentFolder = null;
            CurrentFolderContent = new ObservableCollection<ListElement>();
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

        public string RootFolder { get; set; }

        public string CurrentFolder { get; set; }

        public ObservableCollection<ListElement> CurrentFolderContent { get; set; }

        public async Task TryOpenFolder(ListElement chosenElement)
        {
            if (chosenElement == null || !chosenElement.IsDir)
            {
                return;
            }
            if (!connected)
            {
                HaveMessage?.Invoke(this, "Not connected");
                return;
            }

            try
            {
                var path = CurrentFolder == null ? chosenElement.Name : CurrentFolder += $@"\{chosenElement.Name}";
                var check = await client.ListAsync(path);
                if (check != null)
                {
                    CurrentFolder = path;
                    CurrentFolderContent.Clear();
                    foreach (var element in check)
                    {
                        var name = element.Item1.Substring(element.Item1.LastIndexOf(@"\") + 1);
                        CurrentFolderContent.Add(new ListElement(name, element.Item2));
                    }
                    return;
                }

                HaveMessage?.Invoke(this, "Something went wrong");
            }
            catch (ApplicationException e)
            {
                HaveMessage?.Invoke(this, e.Message);
            }
            catch (SocketException)
            {
                HaveMessage?.Invoke(this, "Not connected");
                return;
            }
        }

        public async Task Back()
        {
            if (CurrentFolder == RootFolder)
            {
                return;
            }
            if (!connected)
            {
                HaveMessage?.Invoke(this, "Not connected");
                return;
            }

            try
            {
                var path = CurrentFolder.Substring(0, CurrentFolder.LastIndexOf(@"\"));
                var check = await client.ListAsync(path);
                if (check != null)
                {
                    CurrentFolder = path;
                    CurrentFolderContent.Clear();
                    foreach (var element in check)
                    {
                        var name = element.Item1.Substring(element.Item1.LastIndexOf(@"\") + 1);
                        CurrentFolderContent.Add(new ListElement(name, element.Item2));
                    }
                    return;
                }

                HaveMessage?.Invoke(this, "Something went wrong");
            }
            catch (ApplicationException e)
            {
                HaveMessage?.Invoke(this, e.Message);
            }
            catch (SocketException)
            {
                HaveMessage?.Invoke(this, "Not connected");
                return;
            }
        }

        public class ListElement
        {
            private string name;
            private bool isDir;
            private string imagePath;

            public ListElement(string name, bool isDir)
            {
                this.name = name;
                this.isDir = isDir;

                imagePath = isDir ? "../Pictures/Downloaded.png" : "../Pictures/Downloaded.png";
            }

            public string Name => name;
            public bool IsDir { get => isDir; set => isDir = value; }
            public string ImagePath { get => imagePath; }
        }
    }
}