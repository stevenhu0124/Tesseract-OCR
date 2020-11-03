using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Tesseract_OCR.Controller;

namespace Tesseract_OCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly CaputreScreen capture = new CaputreScreen(1920, 1080);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _captureText;

        /// <summary>
        /// the degree of the current wheel color selected.
        /// </summary>
        public string CaputreText
        {
            get
            {
                return _captureText;
            }
            set
            {
                if (_captureText != value)
                {
                    _captureText = value;
                    NotifyPropertyChanged(nameof(CaputreText));
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        private void StartDetect(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    CaputreText = capture.ScanCoordinate();
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
