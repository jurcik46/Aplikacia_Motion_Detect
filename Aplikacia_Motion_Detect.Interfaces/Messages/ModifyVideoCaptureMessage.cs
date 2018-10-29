using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Messages
{
    public class ModifyVideoCaptureMessage
    {
        private VideoInfoDataGridModel _videoSource;

        public VideoInfoDataGridModel VideoSource
        {
            get { return _videoSource; }
            set { _videoSource = value; }
        }
    }
}
