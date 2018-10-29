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

namespace Aplikacia_Motion_Detect.UI.Views.MotionZones
{
    public partial class VideoDisplayMotionZones : UserControl
    {
        public VideoDisplayMotionZones()
        {
            InitializeComponent();
            this.axVideoDisplayControl1.ShowMotionZones = true;

            Messenger.Default.Register<MotionZoneMessage>(this, (message) =>
            {
                if (message.VideoSource != null)
                {
                    this.axVideoDisplayControl1.VideoCaptureSource = message.VideoSource;
                }
            });

            Messenger.Default.Register<MotionZoneMessage>(this, (message) =>
            {
                if (message.ShowSelectedZone != null && message.Zone != null && message.ShowSelectedZone == true)
                {
                    ShowSelectedZone(message.Zone);
                }
            });
        }

        Point dragStartPoint = Point.Empty;

        private void axVideoDisplayControl1_VideoMouseDown(object sender, AxDTKVideoCapLib._IVideoDisplayControlEvents_VideoMouseDownEvent e)
        {
            dragStartPoint = new Point(e.x, e.y);
        }

        private void axVideoDisplayControl1_VideoMouseUp(object sender, AxDTKVideoCapLib._IVideoDisplayControlEvents_VideoMouseUpEvent e)
        {
            if (dragStartPoint == Point.Empty)
                return;

            MotionZone zone = new MotionZone();

            // allow minimum zone 5x5
            if (e.x - dragStartPoint.X > 5 && e.y - dragStartPoint.Y > 5)
            {
                zone.X = dragStartPoint.X;
                zone.Y = dragStartPoint.Y;
                zone.Width = e.x - dragStartPoint.X + 1;
                zone.Height = e.y - dragStartPoint.Y + 1;
                dragStartPoint = Point.Empty;
                Messenger.Default.Send<MotionZoneMessage>(new MotionZoneMessage()
                {
                    Zone = zone
                });
                ShowSelectedZone(zone);
            }
        }

        private void ShowSelectedZone(MotionZone zone)
        {
            axVideoDisplayControl1.Overlays.Clear();

            if (zone != null)
            {
                // Add Box overlay to video display control to see the selected motion zone.        
                BoxOverlay box = new BoxOverlay();
                box.X = zone.X;
                box.Y = zone.Y;
                box.Width = zone.Width;
                box.Height = zone.Height;
                box.Color = (uint)ColorTranslator.ToOle(Color.Green);
                box.LineThickness = 8;
                box.Opacity = 80;
                axVideoDisplayControl1.Overlays.Add(box);
            }
        }


    }
}
