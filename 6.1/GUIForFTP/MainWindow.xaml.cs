using System.Windows;
using System.Windows.Controls;

namespace GUIforFTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Start the process, bind with model and list
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            var model = new ViewModel();
            DataContext = model;

            model.HaveMessage += ShowMessage;
        }


        private void ShowMessage(object sender, string message)
        {
            MessageBox.Show(message);
        }

        private void ConnectClick(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).Connect();
        }

        private async void OpenClick(object sender, RoutedEventArgs e)
        {
            await (DataContext as ViewModel).Open(new ViewModel.ListElement((DataContext as ViewModel).RootFolder, true));
        }

        private async void ElementIsChosen(object sender, SelectionChangedEventArgs e)
        {
            await (DataContext as ViewModel).Open((ViewModel.ListElement)listBox.SelectedItem);
        }

        private async void BackClick(object sender, RoutedEventArgs e)
        {
            await (DataContext as ViewModel).Back();
        }

        private async void DownloadClick(object sender, RoutedEventArgs e)
        {
            await (DataContext as ViewModel).Download();
        }
    }
}