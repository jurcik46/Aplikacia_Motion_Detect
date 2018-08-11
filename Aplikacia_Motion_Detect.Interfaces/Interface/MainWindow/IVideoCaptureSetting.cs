using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Interface.MainWindow
{
    public interface IVideoCaptureSetting
    {
        string Name { get; set; }
        string Description { get; set; }
        string State { get; set; }
        string LastError { get; set; }
        string Type { get; set; }
        string Resolution { get; set; }
        string PixelFormat { get; set; }
        string Frames { get; set; }
        string Fps { get; set; }
        string Active { get; set; }

    }
}
