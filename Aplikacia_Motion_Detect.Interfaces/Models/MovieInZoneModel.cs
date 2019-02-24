using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Models
{
    public class MovieInZoneModel
    {
        public DateTime Time { get; set; }
        public string VideoDeviceName { get; set; }
        public string VideoDeviceDescription { get; set; }
        public string ZoneName { get; set; }
    }
}
