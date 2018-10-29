using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class MotionZoneMessage
    {
        private bool? _showSelectedZone;
        private MotionZone _motionZone;
        private VideoCapture _videoSource;

        public VideoCapture VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value; }
        }

        public MotionZone Zone
        {
            get { return _motionZone; }
            set { _motionZone = value; }
        }

        public bool? ShowSelectedZone
        {
            get { return _showSelectedZone; }
            set { _showSelectedZone = value; }
        }
    }
}
