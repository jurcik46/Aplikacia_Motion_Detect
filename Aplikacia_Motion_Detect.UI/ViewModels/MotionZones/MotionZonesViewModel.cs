using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using Aplikacia_Motion_Detect.Interfaces.Models;
using Aplikacia_Motion_Detect.Interfaces.Service;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MotionZones
{
    public class MotionZonesViewModel : ViewModelBase
    {
        #region Command Declaration

        private RelayCommand<IClosable> _okCommand;
        private RelayCommand _addCommand;
        private RelayCommand _deleteCommand;

        public RelayCommand<IClosable> OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        public RelayCommand AddCommand
        {
            get { return _addCommand; }
            set { _addCommand = value; }
        }

        public RelayCommand DeleteCommand
        {
            get { return _deleteCommand; }
            set { _deleteCommand = value; }
        }

        #endregion

        private MotionZoneInfoDataGridModel _selectedMotionZone;
        private IVideoService VideoService { get; set; }
        private VideoInfoDataGridModel _videoDevice;
        public VideoInfoDataGridModel VideoDevice
        {
            get { return _videoDevice; }
            set
            {
                _videoDevice = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<MotionZoneInfoDataGridModel> MotionZoneList { get; set; }

        public MotionZoneInfoDataGridModel SelectedMotionZone
        {
            get { return _selectedMotionZone; }
            set
            {
                _selectedMotionZone = value;
                RaisePropertyChanged();
            }
        }


        public MotionZonesViewModel(IVideoService videoService, VideoInfoDataGridModel selectedDevice)
        {
            this.VideoDevice = selectedDevice;
            MotionZoneList = new ObservableCollection<MotionZoneInfoDataGridModel>();
            VideoService = videoService;
            this.CommandInit();
            this.MessageRegistration();

            for (int i = 0; i < VideoDevice.VideoCapture.MotionZones.Count; i++)
            {
                MotionZone zone = VideoDevice.VideoCapture.MotionZones.Item[i];
                string name = "Zone " + MotionZoneList.Count;
                MotionZoneList.Add(new MotionZoneInfoDataGridModel()
                {
                    Name = name,
                    Zone = zone,
                });
            }
            SetSelectToLast();
        }

        private void SetSelectToLast()
        {
            if (MotionZoneList.Count != 0)
            {
                SelectedMotionZone = MotionZoneList.Last();
            }
        }


        #region Messages Registration
        private void MessageRegistration()
        {
        }
        #endregion

        #region Commands

        private void CommandInit()
        {
            this.OkCommand = new RelayCommand<IClosable>(this.ExecuteOk);
            this.AddCommand = new RelayCommand(this.AddMotionZone);
            this.DeleteCommand = new RelayCommand(this.DeleteMotionZone, this.CanDeleteMotionZone);
        }

        private void ExecuteOk(IClosable win)
        {
            VideoService.SaveConfig();
            if (win != null)
            {
                win.Close();
            }
        }

        private void AddMotionZone()
        {
            MotionZone zone = new MotionZone();
            zone.X = 5;
            zone.Y = 5;
            zone.Width = 100;
            zone.Height = 100;
            string name = "Zone " + MotionZoneList.Count;
            MotionZoneList.Add(new MotionZoneInfoDataGridModel()
            {
                Name = name,
                Zone = zone,
            });

            VideoDevice.VideoCapture.MotionZones.Add(zone);
            SetSelectToLast();
        }

        private bool CanDeleteMotionZone()
        {
            return MotionZoneList.Count != 0 && SelectedMotionZone != null;
        }

        private void DeleteMotionZone()
        {
            VideoDevice.VideoCapture.MotionZones.Remove(SelectedMotionZone.Zone);
            MotionZoneList.Remove(SelectedMotionZone);
            SetSelectToLast();

        }
        #endregion
    }
}