using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;

namespace Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice
{
    public class VideoCaptureViewModel : ViewModelBase
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

        private VideoInfoDataGridModel _selectedDevice;
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

        public VideoInfoDataGridModel SelectedDevice
        {
            get { return _selectedDevice; }
            set { _selectedDevice = value; }
        }
        /**
         * Constructor
         */
        public VideoCaptureViewModel(IVideoService videoService, VideoInfoDataGridModel selectedDevice = null)
        {
            this.VideoDeviceOutputFormat = new ObservableCollection<string>();
            this.VideoDeviceOutputSize = new ObservableCollection<string>();
            this.FoundVideoDevice();
            this.CommandInit();
            this.MessagessRegistration();
            VideoService = videoService;
            SelectedDevice = selectedDevice;
            VideoCaptureName = "New Video Source";
            LoadVideoCapture();
        }

        private void FoundVideoDevice()
        {
            VideoDevicesList = new ObservableCollection<VideoDevice>();
            VideoCaptureUtils utils = new VideoCaptureUtilsClass();
            for (int i = 0; i < utils.VideoDevices.Count; i++)
            {
                VideoDevicesList.Add(utils.VideoDevices.get_Item(i));
            }

        }

        public void SaveVideoCaptureSetting()
        {
            bool Modifie = true;
            if (SelectedDevice == null)
            {
                Modifie = false;
                SelectedDevice = new VideoInfoDataGridModel();

                SelectedDevice.VideoCapture = new VideoCapture();

                if (IsIpCameraCheck)
                {
                    IPCamera ipCamera = new IPCamera();
                    ipCamera.IPCameraURL = IpCameraUrl;
                    ipCamera.IPCameraUser = IpCameraUser;
                    ipCamera.IPCameraPassword = IpCameraPassword;
                    SelectedDevice.VideoCapture.VideoSource = ipCamera;
                }
                else if (IsVideoDeviceCheck)
                {
                    if (SVideoDevice != null)
                    {
                        SelectedDevice.VideoCapture.VideoSource = SVideoDevice;
                    }

                    if (SVideoDeviceOutputSize != null)
                    {
                        SVideoDevice.OutputSize = SVideoDeviceOutputSize;
                    }

                    if (SVideoDeviceOutputFormat != null)
                    {
                        SVideoDevice.OutputPixelFormat = SVideoDeviceOutputFormat;
                    }
                }
                else if (IsVideoFileCheck)
                {
                    VideoFile videoFile = new VideoFile();
                    videoFile.FileName = VideoFilePath;
                    videoFile.Repeat = VideoFileRepeat;
                    SelectedDevice.VideoCapture.VideoSource = videoFile;
                }
            }
            else
            {
                if (IsIpCameraCheck)
                {
                    IPCamera ipCamera = SelectedDevice.VideoCapture.VideoSource is IPCamera
                        ? (IPCamera)SelectedDevice.VideoCapture.VideoSource
                        : new IPCamera();
                    ipCamera.IPCameraURL = IpCameraUrl;
                    ipCamera.IPCameraUser = IpCameraUser;
                    ipCamera.IPCameraPassword = IpCameraPassword;
                }
                else if (IsVideoDeviceCheck)
                {

                    if (SVideoDevice != null)
                    {
                        SelectedDevice.VideoCapture.VideoSource = SVideoDevice;
                    }

                    if (SVideoDeviceOutputSize != null)
                    {
                        SVideoDevice.OutputSize = SVideoDeviceOutputSize;
                    }

                    if (SVideoDeviceOutputFormat != null)
                    {
                        SVideoDevice.OutputPixelFormat = SVideoDeviceOutputFormat;
                    }
                }
                else if (IsVideoFileCheck)
                {
                    VideoFile videoFile = SelectedDevice.VideoCapture.VideoSource is VideoFile
                        ? (VideoFile)SelectedDevice.VideoCapture.VideoSource
                        : new VideoFile();
                    videoFile.FileName = VideoFilePath;
                    videoFile.Repeat = VideoFileRepeat;
                    SelectedDevice.VideoCapture.VideoSource = videoFile;
                }
            }

            SelectedDevice.VideoCapture.Name = VideoCaptureName;
            if (Modifie)
            {
                VideoService.ModifyVideoCapture(SelectedDevice);
            }
            else
            {
                VideoService.AddVideoCapture(SelectedDevice);
            }
        }

        private void LoadVideoCapture()
        {
            if (SelectedDevice != null)
            {
                VideoCaptureName = SelectedDevice.VideoCapture.Name;

                if (SelectedDevice.VideoCapture.VideoSource is IPCamera)
                {
                    IsIpCameraCheck = true;
                    IPCamera ipCamera = (IPCamera)SelectedDevice.VideoCapture.VideoSource;
                    IpCameraUrl = ipCamera.IPCameraURL;
                    IpCameraUser = ipCamera.IPCameraUser;
                    IpCameraPassword = ipCamera.IPCameraPassword;
                }
                else if (SelectedDevice.VideoCapture.VideoSource is VideoDevice)
                {
                    IsVideoDeviceCheck = true;
                    VideoDevice videoDevice = (VideoDevice)SelectedDevice.VideoCapture.VideoSource;

                    foreach (VideoDevice device in VideoDevicesList)
                    {
                        if (device.Path == videoDevice.Path)
                        {
                            SVideoDevice = videoDevice;
                            SVideoDeviceOutputFormat = videoDevice.OutputPixelFormat;
                            SVideoDeviceOutputSize = videoDevice.OutputSize;
                            break;
                        }
                    }
                }
                else if (SelectedDevice.VideoCapture.VideoSource is VideoFile)
                {
                    IsVideoFileCheck = true;

                    VideoFile video = (VideoFile)SelectedDevice.VideoCapture.VideoSource;
                    VideoFilePath = video.FileName;
                    VideoFileRepeat = video.Repeat;
                }

            }
        }


        #region Messagess Registration
        public void MessagessRegistration()
        {
        }
        #endregion

        #region Commands 
        private void CommandInit()
        {
            this.SavieVideoCaptureCommand = new RelayCommand<IClosable>(this.SaveVideoCapture, this.CanSaveVideoCapture);
            this.CloseVideoCaptureCommand = new RelayCommand<IClosable>(this.CloseVideoCapture, this.CanCloseVideoCapture);
            this.DeviceProperties = new RelayCommand(this.ShowDeviceProperties, this.CanShowDeviceProperties);
            this.DeviceSelectionChangedCommand = new RelayCommand(this.ShowDeviceOutputProperties, this.CanDeviceSelectionChangeCommand);
            this.DialogFileCommand = new RelayCommand(this.ShowDialogFile, true);
        }

        private bool CanSaveVideoCapture(IClosable win)
        {
            return true;
        }

        private void SaveVideoCapture(IClosable win)
        {
            SaveVideoCaptureSetting();
            Messenger.Default.Send<ReloadDeviceMessage>(new ReloadDeviceMessage());
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
