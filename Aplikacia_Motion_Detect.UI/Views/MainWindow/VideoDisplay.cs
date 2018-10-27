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
            this.MessagesRegistration();
            this.axVideoDisplayControl1.VideoCaptureSource = null;
            // Create Text overlay to display current date/time

            dateTimeOverlay = new TextOverlay();
            dateTimeOverlay.Text = "";
            dateTimeOverlay.X = 10;
            dateTimeOverlay.Y = 5;
            dateTimeOverlay.FontFile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts) + "\\arial.ttf";
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

        #region Messages Registration
        private void MessagesRegistration()
        {
            Messenger.Default.Register<ShowVideoCaptureMessage>(this, (message) =>
            {
                if (message.ShowVideoCapture)
                {
                    this.axVideoDisplayControl1.VideoCaptureSource = message.Capture;
                    return;
                }
                this.axVideoDisplayControl1.VideoCaptureSource = null;

            });

            Messenger.Default.Register<ShowMotionZonesVideoCaptureMessage>(this, (message) =>
            {
                this.axVideoDisplayControl1.ShowMotionZones = message.Show;
            });
        }
        #endregion
    }
}
