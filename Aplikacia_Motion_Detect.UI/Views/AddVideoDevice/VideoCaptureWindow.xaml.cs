using System.Windows;
using Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice;
namespace Aplikacia_Motion_Detect.UI.Views.AddVideoDevice
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
