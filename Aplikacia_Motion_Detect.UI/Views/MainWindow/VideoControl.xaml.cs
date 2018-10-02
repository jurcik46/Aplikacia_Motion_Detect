using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Forms;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    /// <summary>
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : System.Windows.Controls.UserControl
    {

        public VideoControl()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.VideoViewModel;
            

        }

   
    }
}
