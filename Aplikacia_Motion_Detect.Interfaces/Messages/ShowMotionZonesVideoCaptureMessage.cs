using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class ShowMotionZonesVideoCaptureMessage
    {

        private bool _show;

        public bool Show
        {
            get { return _show; }
            set { _show = value; }
        }
    }
}
