using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    public partial class VideoDisplay : UserControl
    {
        private TextOverlay dateTimeOverlay = null;

        public VideoDisplay()
        {
            InitializeComponent();
            // Create Text overlay to display current date/time

            dateTimeOverlay = new TextOverlay();
            dateTimeOverlay.Text = "";
            dateTimeOverlay.X = 10;
            dateTimeOverlay.Y = 5;
            dateTimeOverlay.FontFile =
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "\\arial.ttf";
            dateTimeOverlay.FontSize = 14;
            dateTimeOverlay.FontColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
            dateTimeOverlay.DrawBox = true;
            dateTimeOverlay.BoxColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Gray);
            dateTimeOverlay.Opacity = 50;
            dateTimeOverlay.ShowDateTime = true;
            dateTimeOverlay.DateTimeFormat = "%m/%d/%Y\n%I:%M:%S %p";

            axVideoDisplayControl1.Overlays.Add(dateTimeOverlay);
            axVideoDisplayControl1.UpdateOverlays();
        }

        public void SetMotionZone(bool sh = false)
        {
            axVideoDisplayControl1.ShowMotionZones = sh;

        }

        public void SetVideoCapt(VideoCapture vidCap = null)
        {
            axVideoDisplayControl1.VideoCaptureSource = vidCap;
        }


    }
}
