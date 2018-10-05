using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    public partial class VideoDisplay : UserControl
    {
        public VideoDisplay()
        {
            InitializeComponent();

            this.axVideoDisplayControl1.VideoCaptureSource =
                ViewModelLocator.VideoViewModel.VideoDispplay.VideoCaptureSource;
        }
    }
}
