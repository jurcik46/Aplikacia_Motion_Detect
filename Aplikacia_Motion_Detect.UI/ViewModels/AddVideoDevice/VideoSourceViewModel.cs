using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice
{
    public class VideoSourceViewModel : ViewModelBase
    {
        public IVideoService VideoService { get; private set; }

        #region Properties for Video File

        private string _videoFilePath;
        public bool VideoFileRepeat { get; set; }
        public RelayCommand DialogFileCommand { get; private set; }
        private bool _isVideoFileCheck;

        public bool IsVideoFileCheck
        {
            get { return _isVideoFileCheck; }
            set
            {
                _isVideoFileCheck = value;
                RaisePropertyChanged();
            }
        }

        public string VideoFilePath
        {
            get { return _videoFilePath; }
            set
            {
                _videoFilePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Properties for IP Camera

        private bool _isIpCameraCheck;
        private string _ipCameraUrl;
        private string _ipCameraUser;
        private string _ipCameraPassword;

        public bool IsIpCameraCheck
        {
            get { return _isIpCameraCheck; }
            set
            {
                _isIpCameraCheck = value;
                RaisePropertyChanged();
            }
        }

        public string IpCameraUrl
        {
            get { return _ipCameraUrl; }
            set
            {
                _ipCameraUrl = value;
                RaisePropertyChanged();
            }
        }

        public string IpCameraUser
        {
            get { return _ipCameraUser; }
            set
            {
                _ipCameraUser = value;
                RaisePropertyChanged();
            }
        }

        public string IpCameraPassword
        {
            get { return _ipCameraPassword; }
            set
            {
                _ipCameraPassword = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Properties for Video Device

        public RelayCommand DeviceProperties { get; private set; }
        public RelayCommand DeviceSelectionChangedCommand { get; private set; }

        public ObservableCollection<VideoDevice> VideoDevicesList
        {
            get => _videoDevicesList;
            set => _videoDevicesList = value;
        }

        public ObservableCollection<string> VideoDeviceOutputFormat
        {
            get { return _videoDeviceOutputFormat; }
            set
            {
                _videoDeviceOutputFormat = value;
                //RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> VideoDeviceOutputSize
        {
            get { return _videoDeviceOutputSize; }
            set
            {
                _videoDeviceOutputSize = value;
                //RaisePropertyChanged(); for Observable Collection this is done automatic
            }
        }

        public VideoDevice SVideoDevice
        {
            get { return _sVideoDevice; }
            set
            {
                _sVideoDevice = value;
                DeviceProperties.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public string SVideoDeviceOutputSize
        {
            get { return _sVideoDeviceOutputSize; }
            set
            {
                _sVideoDeviceOutputSize = value;
                RaisePropertyChanged();
            }
        }

        public string SVideoDeviceOutputFormat
        {
            get { return _sVideoDeviceOutputFormat; }
            set
            {
                _sVideoDeviceOutputFormat = value;
                RaisePropertyChanged();
            }
        }

        public bool IsVideoDeviceCheck
        {
            get { return _isVideoDeviceCheck; }
            set
            {
                _isVideoDeviceCheck = value;
                RaisePropertyChanged();
            }
        }




        private bool _isVideoDeviceCheck;
        private ObservableCollection<VideoDevice> _videoDevicesList;
        private ObservableCollection<string> _videoDeviceOutputFormat;
        private ObservableCollection<string> _videoDeviceOutputSize;
        private VideoDevice _sVideoDevice;
        private string _sVideoDeviceOutputFormat;
        private string _sVideoDeviceOutputSize;

        #endregion

        /**
         * Constructor
         */

        public VideoSourceViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            this.CommandInit();
            this.FoundVideoDevice();
            this.VideoDeviceOutputFormat = new ObservableCollection<string>();
            this.VideoDeviceOutputSize = new ObservableCollection<string>();
        }

        private void FoundVideoDevice()
        {
            VideoDevicesList = new ObservableCollection<VideoDevice>();
            VideoCaptureUtils utils = new VideoCaptureUtilsClass();
            for (int i = 0; i < utils.VideoDevices.Count; i++)
            {
                _videoDevicesList.Add(utils.VideoDevices.get_Item(i));
            }

        }

        //public void SaveVideoCaptureSetting(bool createNew, string name)
        //{
        //    if (createNew)
        //    {
        //        VideoService.VideoDevice.VideoCapture = new VideoCapture();
        //        VideoService.VideoDevice.VideoCapture.Name = name;
        //        if (IsIpCameraCheck)
        //        {
        //            IPCamera ipCamera = new IPCamera();
        //            ipCamera.IPCameraURL = IpCameraUrl;
        //            ipCamera.IPCameraUser = IpCameraUser;
        //            ipCamera.IPCameraPassword = IpCameraPassword;
        //            VideoService.VideoDevice.VideoCapture.VideoSource = ipCamera;
        //        }
        //        else if (IsVideoDeviceCheck)
        //        {
        //            if (SVideoDevice != null)
        //            {
        //                VideoService.VideoDevice.VideoCapture.VideoSource = SVideoDevice;
        //            }

        //            if (SVideoDeviceOutputSize != null)
        //            {
        //                SVideoDevice.OutputSize = SVideoDeviceOutputSize;
        //            }

        //            if (SVideoDeviceOutputFormat != null)
        //            {
        //                SVideoDevice.OutputPixelFormat = SVideoDeviceOutputFormat;
        //            }
        //        }
        //        else if (IsVideoFileCheck)
        //        {
        //            VideoFile videoFile = new VideoFile();
        //            videoFile.FileName = VideoFilePath;
        //            videoFile.Repeat = VideoFileRepeat;
        //            VideoService.VideoDevice.VideoCapture.VideoSource = videoFile;
        //        }
        //    }
        //    else
        //    {
        //        VideoService.VideoDevice.VideoCapture.Name = name;

        //        if (IsIpCameraCheck)
        //        {
        //            IPCamera ipCamera = VideoService.VideoDevice.VideoCapture.VideoSource is IPCamera
        //                ? (IPCamera)VideoService.VideoDevice.VideoCapture.VideoSource
        //                : new IPCamera();
        //            ipCamera.IPCameraURL = IpCameraUrl;
        //            ipCamera.IPCameraUser = IpCameraUser;
        //            ipCamera.IPCameraPassword = IpCameraPassword;
        //            VideoService.VideoDevice.VideoCapture.VideoSource = ipCamera;
        //        }
        //        else if (IsVideoDeviceCheck)
        //        {
        //            if (SVideoDevice != null)
        //            {
        //                VideoService.VideoDevice.VideoCapture.VideoSource = SVideoDevice;
        //            }

        //            if (SVideoDeviceOutputSize != null)
        //            {
        //                SVideoDevice.OutputSize = SVideoDeviceOutputSize;
        //            }

        //            if (SVideoDeviceOutputFormat != null)
        //            {
        //                SVideoDevice.OutputPixelFormat = SVideoDeviceOutputFormat;
        //            }
        //        }
        //        else if (IsVideoFileCheck)
        //        {
        //            VideoFile videoFile = VideoService.VideoDevice.VideoCapture.VideoSource is VideoFile
        //                ? (VideoFile)VideoService.VideoDevice.VideoCapture.VideoSource
        //                : new VideoFile();
        //            videoFile.FileName = VideoFilePath;
        //            videoFile.Repeat = VideoFileRepeat;
        //            VideoService.VideoDevice.VideoCapture.VideoSource = videoFile;
        //        }
        //    }

        //}

        //public void ModifyVideoCapture()
        //{
        //    if (VideoService.VideoDevice != null)
        //    {

        //        if (VideoService.VideoDevice.VideoCapture.VideoSource is IPCamera)
        //        {
        //            IsIpCameraCheck = true;
        //            IPCamera ipCamera = (IPCamera)VideoService.VideoDevice.VideoCapture.VideoSource;
        //            IpCameraUrl = ipCamera.IPCameraURL;
        //            IpCameraUser = ipCamera.IPCameraUser;
        //            IpCameraPassword = ipCamera.IPCameraPassword;
        //        }
        //        else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoDevice)
        //        {
        //            IsVideoDeviceCheck = true;
        //            VideoDevice videoDevice = (VideoDevice)VideoService.VideoDevice.VideoCapture.VideoSource;
        //            SVideoDevice = videoDevice;
        //            SVideoDeviceOutputFormat = videoDevice.OutputPixelFormat;
        //            foreach (VideoDevice device in VideoDevicesList)
        //            {
        //                if (device.Path == videoDevice.Path)
        //                {
        //                    SVideoDevice = videoDevice;
        //                    SVideoDeviceOutputFormat = videoDevice.OutputPixelFormat;
        //                    SVideoDeviceOutputSize = videoDevice.OutputSize;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoFile)
        //        {
        //            IsVideoFileCheck = true;

        //            VideoFile video = (VideoFile)VideoService.VideoDevice.VideoCapture.VideoSource;
        //            VideoFilePath = video.FileName;
        //            VideoFileRepeat = video.Repeat;
        //        }

        //    }
        //}


        #region Commands

        private void CommandInit()
        {
            this.DeviceProperties = new RelayCommand(this.ShowDeviceProperties, this.CanShowDeviceProperties);
            this.DeviceSelectionChangedCommand =
                new RelayCommand(this.ShowDeviceOutputProperties, this.CanDeviceSelectionChangeCommand);
            this.DialogFileCommand = new RelayCommand(this.ShowDialogFile, true);
        }



        private bool CanShowDeviceProperties()
        {
            if (SVideoDevice == null)
            {
                return false;
            }

            return true;
        }

        private void ShowDeviceProperties()
        {
            SVideoDevice.ShowProperties();
        }

        private bool CanDeviceSelectionChangeCommand()
        {
            if (SVideoDevice == null)
                return false;
            return true;
        }

        private void ShowDeviceOutputProperties()
        {

            foreach (var item in SVideoDevice.OutputPixelFormats.Split(';').ToList())
            {

                VideoDeviceOutputFormat.Add(item);
            }

            foreach (var item in SVideoDevice.OutputSizes.Split(';').ToList())
            {
                VideoDeviceOutputSize.Add(item);
            }

        }

        private void ShowDialogFile()
        {
            OpenFileDialog openFileDialg = new OpenFileDialog();
            openFileDialg.Filter = "All Files|*.*|MP4 Files|*.mp4";
            if (openFileDialg.ShowDialog() == true)
            {
                VideoFilePath = openFileDialg.FileName;
            }
        }



        #endregion




    }
}
