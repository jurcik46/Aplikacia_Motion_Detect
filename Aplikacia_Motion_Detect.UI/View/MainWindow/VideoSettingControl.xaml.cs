using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aplikacia_Motion_Detect.UI.ViewModel.MainWindow;

namespace Aplikacia_Motion_Detect.UI.View.MainWindow
{
    /// <summary>
    /// Interaction logic for VideoSettingControl.xaml
    /// </summary>
    public partial class VideoSettingControl : UserControl
    {
        public VideoSettingControl()
        {
            InitializeComponent();
            this.DataContext = new VideoSettingViewModel();
        }
    }
}
