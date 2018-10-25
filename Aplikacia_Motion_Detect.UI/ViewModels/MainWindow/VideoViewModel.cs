using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class VideoViewModel : ViewModelBase
    {
        private ObservableCollection<VideoInfoDataGridModel> _videoSourceList;

        private IVideoService VideoService { get; }

        public VideoInfoDataGridModel SelectedVideoSourceItem { get; set; }

        public ObservableCollection<VideoInfoDataGridModel> VideoSourceList
        {
            get { return _videoSourceList; }
            set { _videoSourceList = value; }
        }

        #region Commands declaration

        private RelayCommand _showVideoCheckedCommand;
        private RelayCommand _showVideoUnCheckedCommand;
        private RelayCommand _showMotionZonesCheckedCommand;
        private RelayCommand _showMotionZonesUnCheckedCommand;

        public RelayCommand ShowVideoCheckedCommand
        {
            get { return _showVideoCheckedCommand; }
            set { _showVideoCheckedCommand = value; }
        }

        public RelayCommand ShowVideoUnCheckedCommand
        {
            get { return _showVideoUnCheckedCommand; }
            set { _showVideoUnCheckedCommand = value; }
        }

        public RelayCommand ShowMotionZonesCheckedCommand
        {
            get { return _showMotionZonesCheckedCommand; }
            set { _showMotionZonesCheckedCommand = value; }
        }

        public RelayCommand ShowMotionZonesUnCheckedCommand
        {
            get { return _showMotionZonesUnCheckedCommand; }
            set { _showMotionZonesUnCheckedCommand = value; }
        }

        #endregion


        public VideoViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            VideoSourceList = new ObservableCollection<VideoInfoDataGridModel>();
            LoadVideoDivaceFromService();
            this.CommandInit();
        }

        public void LoadVideoDivaceFromService()
        {
            VideoSourceList.Clear();

            foreach (var video in VideoService.VideoCaptureList)
            {
                VideoSourceList.Add(video);
            }
        }


        #region Commands

        private void CommandInit()
        {
            this.ShowVideoCheckedCommand = new RelayCommand(this.ShowVideoChecked, this.CanShow);
            this.ShowVideoUnCheckedCommand = new RelayCommand(this.ShowVideoUnChecked, this.CanShow);
            this.ShowMotionZonesCheckedCommand = new RelayCommand(this.ShowMotionZonesChecked, this.CanShow);
            this.ShowMotionZonesUnCheckedCommand = new RelayCommand(this.ShowMotionZonesUnChecked, this.CanShow);
        }

        private bool CanShow()
        {
            return SelectedVideoSourceItem != null;
        }

        private void ShowVideoChecked()
        {
            Messenger.Default.Send<ShowVideoCaptureMessage>(new ShowVideoCaptureMessage()
            {
                Capture = SelectedVideoSourceItem.VideoCapture,
                ShowVideoCapture = true

            });
        }

        private void ShowVideoUnChecked()
        {
            Messenger.Default.Send<ShowVideoCaptureMessage>(new ShowVideoCaptureMessage()
            {
                Capture = null,
                ShowVideoCapture = false

            });
        }

        private void ShowMotionZonesChecked()
        {
            Messenger.Default.Send<ShowMotionZonesVideoCaptureMessage>(new ShowMotionZonesVideoCaptureMessage()
            {
                Show = true
            });
        }

        private void ShowMotionZonesUnChecked()
        {
            Messenger.Default.Send<ShowMotionZonesVideoCaptureMessage>(new ShowMotionZonesVideoCaptureMessage()
            {
                Show = false
            });
        }

        #endregion




    }
}
