using System.Windows;
using Aplikacia_Motion_Detect.UI.ViewModel.VideoCapture;
namespace Aplikacia_Motion_Detect.UI.View.VideoCapture
{
    /// <summary>
    /// Interaction logic for VideoCaptureWindow.xaml
    /// </summary>
    public partial class VideoCaptureWindow : Window
    {
        public VideoCaptureWindow()
        {
            InitializeComponent();
            this.DataContext = new VideoCaptureViewModel();
        }
    }
}
