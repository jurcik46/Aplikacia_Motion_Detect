using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using Aplikacia_Motion_Detect.Interfaces.Enums;
using Aplikacia_Motion_Detect.Interfaces.Extensions;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice;
using Aplikacia_Motion_Detect.UI.ViewModels.DeveloperKey;
using Aplikacia_Motion_Detect.UI.ViewModels.MotionZones;
using Aplikacia_Motion_Detect.UI.Views.AddVideoDevice;
using Aplikacia_Motion_Detect.UI.Views.DeveloperKey;
using Aplikacia_Motion_Detect.UI.Views.MainWindow;
using Aplikacia_Motion_Detect.UI.Views.MotionZones;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Serilog;
using MessageBox = System.Windows.MessageBox;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class MainViewModel : ViewModelBase
    {

        public ILogger Logger => Log.Logger.ForContext<MainViewModel>();

        #region Add Video Capture Window 
        private VideoCaptureViewModel VideoCaptureViewModel;
        private VideoCaptureWindow VideoCaptureWindow;
        #endregion

        #region Developer Key Window
        private DeveloperKeyViewModel DevelopeViewModel;
        private DeveloperKeyWindows DeveloperWindow;
        #endregion

        #region Motion  Zone Window
        private MotionZonesViewModel MotionZoneViewModel;
        private MotionZonesWindow MotionZonesWidow;
        #endregion

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

        public IVideoService VideoService { get; private set; }
        private ObservableCollection<VideoInfoDataGridModel> _videoInfoDataGrid;

        private VideoInfoDataGridModel _selectedDataGridItem;

        public VideoInfoDataGridModel SelectedDataGridItem
        {
            get { return _selectedDataGridItem; }
            set
            {
                _selectedDataGridItem = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<VideoInfoDataGridModel> VideoInfoDataGrid
        {
            get { return _videoInfoDataGrid; }
            set
            {
                _videoInfoDataGrid = value;
            }
        }

        public MainViewModel(IVideoService videoService)
        {
            Logger.Debug(MainViewModelEvents.Create, "Creating new instance of MainViewModel");

            this.VideoService = videoService;
            VideoInfoDataGrid = new ObservableCollection<VideoInfoDataGridModel>();
            this.MessageRegister();
            this.CommandInit();
            this.LoadVideoDeviceFromService();
        }

        private void LoadVideoDeviceFromService()
        {
            Logger.Debug(MainViewModelEvents.LoadVideoDeviceFromService);

            VideoInfoDataGrid.Clear();

            foreach (var video in VideoService.VideoCaptureList)
            {
                VideoInfoDataGrid.Add(video);
            }
            SetSelectToLast();
        }

        private void SetSelectToLast()
        {
            if (VideoInfoDataGrid.Count != 0)
            {
                Logger.Debug(MainViewModelEvents.SetSelectToLast);

                SelectedDataGridItem = VideoInfoDataGrid.Last();
            }
        }


        #region Messages

        private void MessageRegister()
        {
            Messenger.Default.Register<ReloadDeviceMessage>(this, (message) =>
            {
                Logger.Debug(MainViewModelEvents.ReloadDeviceMessage);

                LoadVideoDeviceFromService();
            });
            Messenger.Default.Register<NotifiMessage>(this, (message) => { MessageBox.Show(message.Msg); });
        }
        #endregion


        #region Commands 

        private void CommandInit()
        {
            this.AddVideoCommand = new RelayCommand(this.AddVideoCaptureWindow, this.CanShowVideoCaptureWindow);
            this.ModifyVideoCommand = new RelayCommand(this.ModifyVideoCapture, this.CanModifyVideoCapture);
            this.DeleteVideoCommand = new RelayCommand(this.DeleteVideoCapture, this.CanDeleteVideoCapture);
            this.DeveloperKeyCommand = new RelayCommand(this.ShowDeveloperKeyWindow, this.CanShowDeveloperKeyWindow);
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

        private void AddVideoCaptureWindow()
        {
            Logger.Debug(MainViewModelEvents.AddVideoCommand);

            this.VideoCaptureViewModel = new VideoCaptureViewModel(VideoService);
            this.VideoCaptureWindow = new VideoCaptureWindow() { DataContext = VideoCaptureViewModel };
            VideoCaptureWindow.ShowDialog();
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
            Logger.Information(MainViewModelEvents.ModifyVideoCommand, "Device name {name} \n Setting before \n {@SelectedDataGridItem } ", SelectedDataGridItem.Name);

            this.VideoCaptureViewModel = new VideoCaptureViewModel(VideoService, SelectedDataGridItem);
            this.VideoCaptureWindow = new VideoCaptureWindow() { DataContext = VideoCaptureViewModel };
            this.VideoCaptureWindow.ShowDialog();

        }

        private bool CanDeleteVideoCapture()
        {
            return SelectedDataGridItem != null && this.SelectedDataGridItem.VideoCapture.State == VideoCaptureStateEnum.VCS_Stopped; ;
        }

        private void DeleteVideoCapture()
        {
            Logger.Information(MainViewModelEvents.DeleteVideoCommand, "Device name {name} \n Setting before \n {@SelectedDataGridItem } ", SelectedDataGridItem.Name);
            VideoService.DeleteVideoCapture(SelectedDataGridItem);
            LoadVideoDeviceFromService();
        }

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
            Logger.Debug(MainViewModelEvents.DeveloperKeyCommand);

            this.DevelopeViewModel = new DeveloperKeyViewModel(VideoService);
            this.DeveloperWindow = new DeveloperKeyWindows() { DataContext = DevelopeViewModel };
            this.DeveloperWindow.ShowDialog();
        }

        private bool CanStartCapture()
        {
            return SelectedDataGridItem != null && SelectedDataGridItem.VideoCapture.State == VideoCaptureStateEnum.VCS_Stopped;
        }

        private void StartCaptureVideo()
        {
            Logger.Debug(MainViewModelEvents.StartCaptureCommand, "Device name {name} \n Setting before \n {@SelectedDataGridItem } ", SelectedDataGridItem.Name);
            VideoService.StartCaptureOne(SelectedDataGridItem);
        }

        private bool CanStopCapture()
        {
            return SelectedDataGridItem != null && SelectedDataGridItem.VideoCapture.State != VideoCaptureStateEnum.VCS_Stopped;
        }

        private void StopCaptureVideo()
        {
            Logger.Debug(MainViewModelEvents.StopCaptureCommand, "Device name {name} \n Setting before \n {@SelectedDataGridItem } ", SelectedDataGridItem.Name);
            VideoService.StopCaptureOne(SelectedDataGridItem);
        }

        private bool CanStartCaptureAll()
        {
            return true;
        }

        private void StartCaptureAllVideos()
        {
            Logger.Debug(MainViewModelEvents.StopCaptureAllCommand);
            VideoService.StartCaptureAll();
        }

        private bool CanStopCaptureAll()
        {
            return true;
        }

        private void StopCaptureAllVideos()
        {
            Logger.Debug(MainViewModelEvents.StopCaptureAllCommand);
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

        private void ShowMotionZones()
        {
            Logger.Debug(MainViewModelEvents.DefineMotionZonesCommand);
            this.MotionZoneViewModel = new MotionZonesViewModel(VideoService, SelectedDataGridItem);
            this.MotionZonesWidow = new MotionZonesWindow() { DataContext = MotionZoneViewModel };
            MotionZonesWidow.ShowDialog();
        }
        #endregion
    }
}
