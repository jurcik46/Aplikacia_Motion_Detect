using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class VideoViewModel : ViewModelBase
    {

        private VideoDisplayControl _videoDisplay;

        public VideoDisplayControl VideoDispplay { get => _videoDisplay; set => _videoDisplay = value; }

        public VideoViewModel()
        {

            VideoCapture videoCapture = new VideoCapture();


            ////videoCapture.FrameReceived += FormMain_FrameReceived;


            VideoCaptureUtils utils = new VideoCaptureUtils();
            // enumirate video devices 
            // fill video devices combobox
            for (int i = 0; i < utils.VideoDevices.Count; i++)
            {
                VideoDevice videDev = utils.VideoDevices.get_Item(i);
                Console.WriteLine(videDev.DisplayName);
                videoCapture.VideoSource = videDev;
            }


            // start capture process
            videoCapture.StartCapture();


            VideoDispplay = new VideoDisplayControl();
            VideoDispplay.VideoCaptureSource = videoCapture;

        }

    }
}
