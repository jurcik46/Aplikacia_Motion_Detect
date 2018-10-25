using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class ShowVideoCaptureMessage
    {
        private bool _showVideoCapture;

        private VideoCapture _videoCapture;

        public VideoCapture Capture
        {
            get { return _videoCapture; }
            set { _videoCapture = value; }
        }

        public bool ShowVideoCapture
        {
            get { return _showVideoCapture; }
            set { _showVideoCapture = value; }
        }

    }
}
