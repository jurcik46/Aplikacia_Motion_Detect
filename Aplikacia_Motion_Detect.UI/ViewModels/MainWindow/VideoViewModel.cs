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

        public VideoDisplayControl test;
        public VideoViewModel()
        {
            // create new VideoCapture instance
            VideoCapture videoCapture = new VideoCapture();


            //videoCapture.FrameReceived += FormMain_FrameReceived;


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

  
            test = new VideoDisplayControl();
            test.VideoCaptureSource = videoCapture;
            //host.Child = (System.Windows.Forms.Control)aa;
            //stack.Children.Add(host);
            //Task.Run(() =>
            //{
            //    while(true)
            //    {
            //        Console.WriteLine(videoCapture.State);

            //    }

            //});
        }

    }
}
