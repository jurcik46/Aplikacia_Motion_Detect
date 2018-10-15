using System.Collections.Generic;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Models;

namespace Aplikacia_Motion_Detect.Interfaces.Service
{
    public class VideoService : IVideoService
    {

        public List<VideoInfoDataGridModel> _videoCaptureList;
        public VideoInfoDataGridModel _videoDevice;

        public VideoService()
        {
            this.VideoCaptureList = new List<VideoInfoDataGridModel>();
            //this.VideoDevice = null;
        }

        public void ModifyVideoCapture()
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, VideoDevice))
                {
                    ChangeVideoSource(item);
                    return;
                }
            }
        }

        public void DeleteVideoCapture()
        {
            foreach (var item in VideoCaptureList)
            {
                if (object.ReferenceEquals(item, VideoDevice))
                {
                    DeleteVideoSource(item);
                    return;
                }
            }
        }

        private void ChangeVideoSource(VideoInfoDataGridModel videSource)
        {
            videSource = VideoDevice;
            VideoDevice = null;
        }

        private void DeleteVideoSource(VideoInfoDataGridModel videSource)
        {
            VideoCaptureList.Remove(videSource);
            VideoDevice = null;
        }


        public List<VideoInfoDataGridModel> VideoCaptureList
        {
            get { return _videoCaptureList; }
            set { _videoCaptureList = value; }
        }

        public VideoInfoDataGridModel VideoDevice
        {
            get { return _videoDevice; }
            set { _videoDevice = value; }
        }
    }
}
