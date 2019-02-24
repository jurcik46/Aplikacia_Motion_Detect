using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Enums
{
    public enum VideoServiceEvents
    {
        Create,
        SetDeveloperKey,
        AddVideoCaptureDevice,
        FrameReceived,
        VideoCaptureStateChanged,
        VideoCaptureError,
        VideoDeviceDisconnected,
        TryingRestartCapturing,
        ModifyVideoCapture,
        DeleteVideoCapture,
        StartCapture,
        StopCapture,
        SavingConfig,
        LoadingConfig,
        SettingGUIDeviceDataAll,
        UpdateStateVideoDevice,
        CutImage,
        MovieInZoneDetected,
        VideoZoneTick

    }
}
