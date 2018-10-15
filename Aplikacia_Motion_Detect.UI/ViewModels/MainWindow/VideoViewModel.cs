using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class VideoViewModel : ViewModelBase
    {
        private IVideoService VideoService { get; }

        private VideoDisplayControl _videoDisplay;

        public VideoDisplayControl VideoDispplay { get => _videoDisplay; set => _videoDisplay = value; }

        public VideoViewModel(IVideoService videoService)
        {
            VideoService = videoService;

            VideoCapture videoCapture = new VideoCapture();


            ////videoCapture.FrameReceived += FormMain_FrameReceived;


            VideoCaptureUtils utils = new VideoCaptureUtils();
            // enumirate video devices 
            // fill video devices combobox
            for (int i = 0; i < utils.VideoDevices.Count; i++)
            {
                VideoDevice videDev = utils.VideoDevices.get_Item(i);
                videoCapture.VideoSource = videDev;
            }


            // start capture process
            videoCapture.StartCapture();


            VideoDispplay = new VideoDisplayControl();
            VideoDispplay.VideoCaptureSource = videoCapture;

        }

    }
}
