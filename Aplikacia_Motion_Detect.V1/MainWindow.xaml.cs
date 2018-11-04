using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using Aplikacia_Motion_Detect.UI;

namespace Aplikacia_Motion_Detect.V1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            ViewModelLocator.VideoService.SaveConfig();
        }
    }
}
