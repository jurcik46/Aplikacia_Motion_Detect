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
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class VideoSettingViewModel : ViewModelBase
    {
        private IVideoService VideoService { get; }
        private VideoCaptureWindow VideoCaptureWindow;
        private ObservableCollection<VideoInfoDataGridModel> _videoInfoDataGrid;
        private RelayCommand _addVideoCommand;
        private RelayCommand _modifyVideoCommand;
        private RelayCommand _deleteVideoCommand;
        private RelayCommand _startCaptureCommand;
        private RelayCommand _stopCaptureCommand;
        private RelayCommand _startCaptureAllCommand;
        private RelayCommand _stopCaptureAllCommand;
        private RelayCommand _defineMotionZonesCommand;


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

        public void SaveVideoCapture(bool createNew)
        {
            var pom = VideoService.VideoDevice;

            if (createNew)
            {
                pom = new VideoInfoDataGridModel();
            }

            pom.Name = VideoService.VideoDevice.VideoCapture.Name;
            pom.VideoCapture = VideoService.VideoDevice.VideoCapture;

            string strState = VideoService.VideoDevice.VideoCapture.State.ToString();
            //remove prefix from the state contatnt name 
            strState = strState.Substring(4, strState.Length - 4);
            pom.State = strState;
            if (VideoService.VideoDevice.VideoCapture.VideoSource is IPCamera)
            {
                IPCamera ipPCamera = (IPCamera)VideoService.VideoDevice.VideoCapture.VideoSource;
                pom.Description = ipPCamera.IPCameraURL;
                pom.Type = "IP Camera";
            }
            else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoDevice)
            {
                VideoDevice videoDevice = (VideoDevice)VideoService.VideoDevice.VideoCapture.VideoSource;
                pom.Description = videoDevice.DisplayName;
                pom.Type = "Video Device";
            }
            else if (VideoService.VideoDevice.VideoCapture.VideoSource is VideoFile)
            {
                VideoFile videoFile = (VideoFile)VideoService.VideoDevice.VideoCapture.VideoSource;
                pom.Description = videoFile.FileName;
                pom.Type = "Video File";
            }

            pom.Frames = 0;
            pom.FPS = 0;
            pom.Resolution = "";
            if (createNew)
            {
                VideoService.VideoCaptureList.Add(pom);
            }
            else
            {
                VideoService.ModifyVideoCapture();
            }

            LoadVideoDivaceFromService();
            VideoService.VideoDevice = null;
        }


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
            this.DeleteVideoCommand = new RelayCommand(this.DeleteVideoCapture, this.CanDeleteVideoCapture);
            this.StartCaptureCommand = new RelayCommand(this.StartCaptureVideo, this.CanStartCapture);
            this.StopCaptureCommand = new RelayCommand(this.StopCaptureVideo, this.CanStopCapture);
            this.StartCaptureAllCommand = new RelayCommand(this.StartCaptureAllVideos, this.CanStartCaptureAll);
            this.StopCaptureAllCommand = new RelayCommand(this.StopCaptureAllVideos, this.CanStopCaptureAll);
            this.DefineMotionZonesCommand = new RelayCommand(this.ShowMotionZones, this.CanShowMotionZones);
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

            return SelectedDataGridItem != null;
        }

        private void ModifyVideoCapture()
        {
            this.VideoCaptureWindow = new VideoCaptureWindow();
            this.VideoCaptureWindow.Show();

            Messenger.Default.Send<ModifyVideoCapture>(new ModifyVideoCapture() { VideoSource = SelectedDataGridItem });

        }

        private bool CanDeleteVideoCapture()
        {
            return SelectedDataGridItem != null;
        }

        private void DeleteVideoCapture()
        {
            VideoService.VideoDevice = SelectedDataGridItem;
            VideoService.DeleteVideoCapture();
            VideoInfoDataGrid.Remove(SelectedDataGridItem);

        }

        private bool CanStartCapture()
        {
            return true;
        }

        private void StartCaptureVideo()
        {
        }

        private bool CanStopCapture()
        {
            return true;
        }

        private void StopCaptureVideo()
        {

        }

        private bool CanStartCaptureAll()
        {
            return true;
        }

        private void StartCaptureAllVideos()
        {

        }

        private bool CanStopCaptureAll()
        {
            return true;
        }

        private void StopCaptureAllVideos()
        {

        }

        private bool CanShowMotionZones()
        {
            return true;
        }

        private void ShowMotionZones()
        {

        }


        #endregion



    }
}
