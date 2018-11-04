using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Enums
{
    public enum VideoCaptureViewModelEvents
    {
        Create,
        FoundVideoDevice,
        SaveVideoCaptureSetting,
        LoadVideoCapture,
        SaveVideoCaptureCommand,
        CloseVideoCaptureCommand,
        DevicePropertiesCommand,
        DeviceSelectionChangedCommand,
        DialogFileCommand

    }
}
