using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.UI.Views.AddVideoDevice;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using Aplikacia_Motion_Detect.Interfaces.Service;
using Aplikacia_Motion_Detect.UI.Views.DeveloperKey;
using Aplikacia_Motion_Detect.UI.Views.MotionZones;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class VideoSettingViewModel : ViewModelBase
    {
        private IVideoService VideoService { get; }
        private VideoCaptureWindow VideoCaptureWindow;
        private DeveloperKeyWindows DeveloperWindow;
        private MotionZonesWindow MotionZonesWidow;
        private ObservableCollection<VideoInfoDataGridModel> _videoInfoDataGrid;
        #region Commmand Declaration
        private RelayCommand _addVideoCommand;
        private RelayCommand _modifyVideoCommand;
        private RelayCommand _deleteVideoCommand;
        private RelayCommand _startCaptureCommand;
        private RelayCommand _stopCaptureCommand;
        private RelayCommand _startCaptureAllCommand;
        private RelayCommand _stopCaptureAllCommand;
        private RelayCommand _defineMotionZonesCommand;
        private RelayCommand _developerKeyCommand;


        public RelayCommand DeleteVideoCommand
        {
            get => _deleteVideoCommand;
            set => _deleteVideoCommand = value;
        }

        public RelayCommand ModifyVideoCommand
        {
            get => _modifyVideoCommand;
            set => _modifyVideoCommand = value;
        }

        public RelayCommand AddVideoCommand
        {
            get => _addVideoCommand;
            set => _addVideoCommand = value;
        }

        public RelayCommand StartCaptureCommand
        {
            get { return _startCaptureCommand; }
            set { _startCaptureCommand = value; }
        }

        public RelayCommand StopCaptureCommand
        {
            get { return _stopCaptureCommand; }
            set { _stopCaptureCommand = value; }
        }

        public RelayCommand StartCaptureAllCommand
        {
            get { return _startCaptureAllCommand; }
            set { _startCaptureAllCommand = value; }
        }

        public RelayCommand StopCaptureAllCommand
        {
            get { return _stopCaptureAllCommand; }
            set { _stopCaptureAllCommand = value; }
        }

        public RelayCommand DefineMotionZonesCommand
        {
            get { return _defineMotionZonesCommand; }
            set { _defineMotionZonesCommand = value; }
        }

        public RelayCommand DeveloperKeyCommand
        {
            get { return _developerKeyCommand; }
            set { _developerKeyCommand = value; }
        }

        #endregion

        public VideoInfoDataGridModel SelectedDataGridItem { get; set; }


        public ObservableCollection<VideoInfoDataGridModel> VideoInfoDataGrid
        {
            get { return _videoInfoDataGrid; }
            set
            {
                _videoInfoDataGrid = value;
                //RaisePropertyChanged();
            }
        }

        /**
         * Constructor
         */
        public VideoSettingViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            VideoInfoDataGrid = new ObservableCollection<VideoInfoDataGridModel>();
            this.LoadVideoDivaceFromService();
            this.CommandInit();

        }

        //public void SaveVideoCapture(bool createNew)
        //{
        //    var pom = VideoService.VideoDevice;

        //    if (createNew)
        //    {
        //        pom = new VideoInfoDataGridModel();
        //    }

        //    pom.Name = VideoService.VideoDevice.VideoCapture.Name;
        //    pom.VideoCapture = VideoService.VideoDevice.VideoCapture;

        //    string strState = VideoService.VideoDevice.VideoCapture.State.ToString();
        //    //remove prefix from the state contatnt name 
        //    strState = strState.Substring(4, strState.Length - 4);
        //    pom.State = strState;
        //    if (VideoService.VideoDevice.VideoCapture.VideoSource is IPCamera)
        //    {
        //        IPCamera ipPCamera = (IPCamera)VideoService.VideoDevice.VideoCapture.VideoSource;
        //        pom.Description = ipPCamera.IPCameraURL;
        //        pom.Type = "IP Camera";
        //    }
        //    else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoDevice)
        //    {
        //        VideoDevice videoDevice = (VideoDevice)VideoService.VideoDevice.VideoCapture.VideoSource;
        //        pom.Description = videoDevice.DisplayName;
        //        pom.Type = "Video Device";
        //    }
        //    else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoFile)
        //    {
        //        VideoFile videoFile = (VideoFile)VideoService.VideoDevice.VideoCapture.VideoSource;
        //        pom.Description = videoFile.FileName;
        //        pom.Type = "Video File";
        //    }

        //    pom.Frames = 0;
        //    pom.FPS = 0;
        //    pom.Resolution = "";
        //    if (createNew)
        //    {
        //        VideoService.VideoDevice = pom;
        //        //VideoService.AddVideoCapture();
        //    }
        //    else
        //    {
        //        VideoService.ModifyVideoCapture();
        //    }

        //    LoadVideoDivaceFromService();
        //    VideoService.VideoDevice = null;
        //}


        private void LoadVideoDivaceFromService()
        {
            VideoInfoDataGrid.Clear();

            foreach (var video in VideoService.VideoCaptureList)
            {
                VideoInfoDataGrid.Add(video);
            }
        }


        #region Commands 

        private void CommandInit()
        {
            this.AddVideoCommand = new RelayCommand(this.ShowVideoCaptureWindow, this.CanShowVideoCaptureWindow);
            this.ModifyVideoCommand = new RelayCommand(this.ModifyVideoCapture, this.CanModifyVideoCapture);
            //this.DeleteVideoCommand = new RelayCommand(this.DeleteVideoCapture, this.CanDeleteVideoCapture);
            this.DeveloperKeyCommand = new RelayCommand(this.ShowDeveloperKeyWindow, this.CanShowDeveloperKeyWindow);
            this.StartCaptureCommand = new RelayCommand(this.StartCaptureVideo, this.CanStartCapture);
            this.StopCaptureCommand = new RelayCommand(this.StopCaptureVideo, this.CanStopCapture);
            this.StartCaptureAllCommand = new RelayCommand(this.StartCaptureAllVideos, this.CanStartCaptureAll);
            this.StopCaptureAllCommand = new RelayCommand(this.StopCaptureAllVideos, this.CanStopCaptureAll);
            //this.DefineMotionZonesCommand = new RelayCommand(this.ShowMotionZones, this.CanShowMotionZones);
        }


        private bool CanShowVideoCaptureWindow()
        {
            if (this.VideoCaptureWindow != null)
                return (!this.VideoCaptureWindow.IsLoaded);
            else
            {
                return true;
            }
        }

        private void ShowVideoCaptureWindow()
        {
            this.VideoCaptureWindow = new VideoCaptureWindow();
            this.VideoCaptureWindow.Show();
        }

        private bool CanModifyVideoCapture()
        {
            if (this.VideoCaptureWindow != null)
            {
                if (this.VideoCaptureWindow.IsLoaded)
                    return false;
            }

            return SelectedDataGridItem != null && this.SelectedDataGridItem.VideoCapture.State == VideoCaptureStateEnum.VCS_Stopped;
        }

        private void ModifyVideoCapture()
        {
            this.VideoCaptureWindow = new VideoCaptureWindow();
            this.VideoCaptureWindow.Show();

            Messenger.Default.Send<ModifyVideoCaptureMessage>(new ModifyVideoCaptureMessage() { VideoSource = SelectedDataGridItem });

        }

        private bool CanDeleteVideoCapture()
        {
            return SelectedDataGridItem != null;
        }

        //private void DeleteVideoCapture()
        //{
        //    VideoService.VideoDevice = SelectedDataGridItem;
        //    VideoService.DeleteVideoCapture();
        //    VideoInfoDataGrid.Remove(SelectedDataGridItem);



        //}

        private bool CanShowDeveloperKeyWindow()
        {
            if (this.DeveloperWindow != null)
                return (!this.DeveloperWindow.IsLoaded);
            else
            {
                return true;
            }
        }

        private void ShowDeveloperKeyWindow()
        {
            this.DeveloperWindow = new DeveloperKeyWindows();
            this.DeveloperWindow.Show();
        }

        private bool CanStartCapture()
        {
            return SelectedDataGridItem != null && SelectedDataGridItem.VideoCapture.State == VideoCaptureStateEnum.VCS_Stopped;
        }

        private void StartCaptureVideo()
        {
            VideoService.StartCaptureOne(SelectedDataGridItem);
        }

        private bool CanStopCapture()
        {
            return SelectedDataGridItem != null && SelectedDataGridItem.VideoCapture.State != VideoCaptureStateEnum.VCS_Stopped;
        }

        private void StopCaptureVideo()
        {

            VideoService.StopCaptureOne(SelectedDataGridItem);


        }

        private bool CanStartCaptureAll()
        {
            return true;
        }

        private void StartCaptureAllVideos()
        {
            VideoService.StartCaptureAll();
        }

        private bool CanStopCaptureAll()
        {
            return true;
        }

        private void StopCaptureAllVideos()
        {
            VideoService.StopCaptureAll();
        }

        private bool CanShowMotionZones()
        {
            if (MotionZonesWidow != null)
            {
                return (!MotionZonesWidow.IsLoaded);
            }
            if (SelectedDataGridItem != null &&
                SelectedDataGridItem.VideoCapture.State == VideoCaptureStateEnum.VCS_Started)
            {
                return true;
            }

            return false;

        }

        //private void ShowMotionZones()
        //{
        //    VideoService.VideoDevice = SelectedDataGridItem;
        //    MotionZonesWidow = new MotionZonesWindow();
        //    MotionZonesWidow.Show();
        //}


        #endregion



    }
}
