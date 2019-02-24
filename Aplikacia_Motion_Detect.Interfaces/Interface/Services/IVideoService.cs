using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;


namespace Aplikacia_Motion_Detect.Interfaces.Interface.Services
{
    public interface IVideoService
    {
        List<VideoInfoDataGridModel> VideoCaptureList { get; set; }
        ObservableCollection<MovieInZoneModel> MovieOnVideo { get; set; }
        string DeveloperKey { get; set; }
        void AddVideoCapture(VideoInfoDataGridModel videoDevice);
        void ModifyVideoCapture(VideoInfoDataGridModel videoDevice);
        void DeleteVideoCapture(VideoInfoDataGridModel videoDevice);
        void StartCaptureOne(VideoInfoDataGridModel videoSource);
        void StartCaptureAll();
        void StopCaptureOne(VideoInfoDataGridModel videoSource);
        void StopCaptureAll();
        void SaveConfig();
        void LoadConfig();
        void VideoZoneDispatcherTimer_Tick(VideoInfoDataGridModel video, MotionZoneInfoDataGridModel zone);
        VideoInfoDataGridModel FoundEqualsVideoCapture(VideoCapture vidCap);
    }


}
