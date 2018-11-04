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

        private MotionZone _zone;
        public VideoDisplayMotionZones()
        {
            InitializeComponent();
            this.axVideoDisplayControl1.ShowMotionZones = true;
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

            MotionZone zone = this._zone;

            // allow minimum zone 5x5
            if (e.x - dragStartPoint.X > 5 && e.y - dragStartPoint.Y > 5)
            {
                zone.X = dragStartPoint.X;
                zone.Y = dragStartPoint.Y;
                zone.Width = e.x - dragStartPoint.X + 1;
                zone.Height = e.y - dragStartPoint.Y + 1;
                dragStartPoint = Point.Empty;
                ShowSelectedZone(zone);
            }
        }

        public void SetVideoCapture(VideoCapture videoCapture)
        {
            this.axVideoDisplayControl1.VideoCaptureSource = videoCapture;

        }

        public void SetZone(MotionZone zone)
        {
            this._zone = zone;
            ShowSelectedZone(this._zone);
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
                box.Color = (uint)ColorTranslator.ToOle(Color.GreenYellow);
                box.LineThickness = 5;
                box.Opacity = 100;
                axVideoDisplayControl1.Overlays.Add(box);
                axVideoDisplayControl1.ShowMotionZones = false;
                axVideoDisplayControl1.ShowMotionZones = true;

            }
        }


    }
}
