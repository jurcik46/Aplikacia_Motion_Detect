using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacia_Motion_Detect.Interfaces.Enums
{
    public enum MainViewModelEvents
    {
        Create,
        LoadVideoDeviceFromService,
        SetSelectToLast,
        ReloadDeviceMessage,
        AddVideoCommand,
        ModifyVideoCommand,
        DeleteVideoCommand,
        DeveloperKeyCommand,
        StartCaptureCommand,
        StopCaptureCommand,
        StartCaptureAllCommand,
        StopCaptureAllCommand,
        DefineMotionZonesCommand,

    }
}
