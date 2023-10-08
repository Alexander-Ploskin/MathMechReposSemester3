using System;
using System.Net.Sockets;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace GUIforFTP
{
    /// <summary>
    /// Provides binding between FTPClient and GUI
    /// </summary>
    public class ViewModel
    {
        private int port;
        private FTPClient.FTPClient client;
        private bool connected;

        /// <summary>
        /// Event for messages
        /// </summary>
        public event EventHandler<string> HaveMessage;

        /// <summary>
        /// Constructor with default settings
        /// </summary>
        public ViewModel()
        {
            port = 49001;
            ServerAddress = "localhost";
            connected = false;
            RootFolder = "../../../../";
            CurrentFolder = null;
            NameOfItemToDownload = "dir1";
            PathToDownload = "../../../../GUIForFTP";
            CurrentFolderContent = new ObservableCollection<ListElement>();
            DownloadedElements = new ObservableCollection<ListElement>();
        }

        /// <summary>
        /// Port to connect with the server
        /// </summary>
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

        /// <summary>
        /// Adress to connect with the server
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// Creates new Instanse of FTPClient with connection with the server
        /// </summary>
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

        /// <summary>
        /// Path of the root folder in the server
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// Path of the current folder in the server
        /// </summary>
        public string CurrentFolder { get; set; }

        /// <summary>
        /// Collection of items in the current directory
        /// </summary>
        public ObservableCollection<ListElement> CurrentFolderContent { get; set; }

        /// <summary>
        /// Tries to open one of items in the current directory
        /// </summary>
        /// <param name="chosenElement">Element to open</param>
        public async Task Open(ListElement chosenElement)
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

        /// <summary>
        /// Returns to one level higher
        /// </summary>
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

        /// <summary>
        /// Name of the item to download
        /// </summary>
        public string NameOfItemToDownload { get; set; }

        /// <summary>
        /// Directory in the client's computer to download files
        /// </summary>
        public string PathToDownload { get; set; }

        /// <summary>
        /// Collection of downloaded items
        /// </summary>
        public ObservableCollection<ListElement> DownloadedElements { get; set; }

        /// <summary>
        /// Downloads file or all files in the folder from the server
        /// </summary>
        public async Task Download()
        {
            if (!connected)
            {
                HaveMessage?.Invoke(this, "Not connected");
                return;
            }

            if (NameOfItemToDownload == "")
            {
                HaveMessage?.Invoke(this, "Enter the name of item to download");
                return;
            }

            if (PathToDownload == "")
            {
                HaveMessage?.Invoke(this, "Set the path to download");
                return;
            }

            var wantedItem = CurrentFolderContent.Where(i => i.Name == NameOfItemToDownload).FirstOrDefault();
            if (wantedItem == null)
            {
                HaveMessage?.Invoke(this, "There is not item with that name");
                return;
            }
            if (wantedItem.IsDir)
            {
                var pathOfNewDir = PathToDownload + $@"\{NameOfItemToDownload}";
                Directory.CreateDirectory(pathOfNewDir);
                var itemsInDir = await client.ListAsync(CurrentFolder + $@"\{NameOfItemToDownload}");
                Connect();
                foreach (var item in itemsInDir)
                {
                    var newElement = new ListElement(item.Item1.Substring(item.Item1.LastIndexOf(@"\") + 1), false);
                    DownloadedElements.Add(newElement);
                    Task.Run(async () => await client.GetAsync(item.Item1, pathOfNewDir, item.Item1.Substring(item.Item1.LastIndexOf(@"\") + 1)));
                }
                return;
            }
            DownloadedElements.Add(new ListElement(wantedItem.Name, false));
            await Task.Run(async () => await client.GetAsync(CurrentFolder + $@"\{NameOfItemToDownload}", PathToDownload, wantedItem.Name));
        }

        /// <summary>
        /// Class of items in directories
        /// </summary>
        public class ListElement
        {
            private string name;
            private bool isDir;
            private string imagePath;

            public ListElement(string name, bool isDir)
            {
                this.name = name;
                this.isDir = isDir;

                imagePath = isDir ? "Resourses/folder.png" : "Resourses/file.png";
            }

            public string Name => name;
            public bool IsDir { get => isDir; set => isDir = value; }
            public string ImagePath { get => imagePath; }
        }
    }
}