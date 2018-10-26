using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Models;

namespace Aplikacia_Motion_Detect.Interfaces.Interface.Services
{
    public interface IVideoService
    {
        List<VideoInfoDataGridModel> VideoCaptureList { get; set; }
        VideoInfoDataGridModel VideoDevice { get; set; }
        string DeveloperKey { get; set; }
        void AddVideoCapture();
        void ModifyVideoCapture();
        void DeleteVideoCapture();
        void StartCaptureOne(VideoInfoDataGridModel videoSource);
        void StartCaptureAll();
        void StopCaptureOne(VideoInfoDataGridModel videoSource);
        void StopCaptureAll();
        void SaveConfig();
    }


}
