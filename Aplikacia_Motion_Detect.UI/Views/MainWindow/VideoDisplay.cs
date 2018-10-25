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
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.Views.MainWindow
{
    public partial class VideoDisplay : UserControl
    {
        public VideoDisplay()
        {
            InitializeComponent();
            this.MessagesRegistration();
            this.axVideoDisplayControl1.VideoCaptureSource = null;
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
