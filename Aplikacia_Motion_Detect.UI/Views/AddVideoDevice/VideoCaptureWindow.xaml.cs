using System.ComponentModel;
using System.Windows;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice;
namespace Aplikacia_Motion_Detect.UI.Views.AddVideoDevice
{
    /// <summary>
    /// Interaction logic for VideoCaptureWindow.xaml
    /// </summary>
    public partial class VideoCaptureWindow : Window, IClosable
    {
        public VideoCaptureWindow()
        {
            InitializeComponent();
        }

        private void VideoCaptureWindow_OnClosing(object sender, CancelEventArgs e)
        {
        }
    }
}
