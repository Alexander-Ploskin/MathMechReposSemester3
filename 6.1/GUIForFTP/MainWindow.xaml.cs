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
    }
}