using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using Aplikacia_Motion_Detect.Interfaces.Service;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
namespace Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice
{
    public class VideoCaptureViewModel : ViewModelBase
    {
        public IVideoService VideoService { get; private set; }

        #region Sub ViewModels
        public VideoSourceViewModel VideoSourceControl { get; set; }
        public SaveOutputViewModel SaveOutputControl { get; set; }

        #endregion

        private VideoCapture _videoCapture;
        private string _videoCaptureName;

        public string VideoCaptureName
        {
            get { return _videoCaptureName; }
            set
            {
                _videoCaptureName = value;
                RaisePropertyChanged();
            }
        }


        public RelayCommand<IClosable> SavieVideoCaptureCommand { get; private set; }
        public RelayCommand<IClosable> CloseVideoCaptureCommand { get; private set; }

        public VideoCapture VideCapture
        {
            get { return _videoCapture; }
            set { _videoCapture = value; }
        }
        /**
         * Constructor
         */
        public VideoCaptureViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            VideoCaptureName = "New Video Source";
            SaveOutputControl = new SaveOutputViewModel(videoService);
            VideoSourceControl = new VideoSourceViewModel(videoService);
            this.CommandInit();
            this.MessagessRegistration();


        }

        #region Messagess Registration

        public void MessagessRegistration()
        {
            Messenger.Default.Register<ModifyVideoCaptureMessage>(this, (message) =>
            {
                VideoService.VideoDevice = message.VideoSource;
                VideoCaptureName = VideoService.VideoDevice.Name;
                VideoSourceControl.ModifyVideoCapture();
            });
        }


        #endregion

        #region Commands 
        private void CommandInit()
        {
            this.SavieVideoCaptureCommand = new RelayCommand<IClosable>(this.SaveVideoCapture, this.CanSaveVideoCapture);
            this.CloseVideoCaptureCommand = new RelayCommand<IClosable>(this.CloseVideoCapture, this.CanCloseVideoCapture);
        }

        private bool CanSaveVideoCapture(IClosable win)
        {
            return true;
        }

        private void SaveVideoCapture(IClosable win)
        {
            if (VideoService.VideoDevice == null)
            {
                VideoService.VideoDevice = new VideoInfoDataGridModel();
                VideoSourceControl.SaveVideoCaptureSetting(true, VideoCaptureName);
                Messenger.Default.Send<AddVideoCaptureMessage>(new AddVideoCaptureMessage()
                {
                    CreateNew = true
                });
            }
            else
            {
                VideoSourceControl.SaveVideoCaptureSetting(false, VideoCaptureName);
                Messenger.Default.Send<AddVideoCaptureMessage>(new AddVideoCaptureMessage()
                {
                    CreateNew = false
                });
            }




            if (win != null)
            {
                win.Close();
            }

        }

        private bool CanCloseVideoCapture(IClosable win)
        {
            return true;
        }

        private void CloseVideoCapture(IClosable win)
        {

            if (win != null)
            {
                win.Close();
            }
        }
        #endregion

    }



}
