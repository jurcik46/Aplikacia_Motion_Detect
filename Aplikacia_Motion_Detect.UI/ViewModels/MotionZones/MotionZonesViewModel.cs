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
        private IVideoCapture VideoCapture { get; set; }

        public ObservableCollection<MotionZoneInfoDataGridModel> MotionZoneList { get; set; }

        public MotionZoneInfoDataGridModel SelectedMotionZone
        {
            get { return _selectedMotionZone; }
            set
            {
                _selectedMotionZone = value;
                Messenger.Default.Send<MotionZoneMessage>(new MotionZoneMessage()
                {
                    Zone = value.Zone,
                    ShowSelectedZone = true
                });
                RaisePropertyChanged(); // need test 
            }
        }


        public MotionZonesViewModel(IVideoService videoService)
        {
            MotionZoneList = new ObservableCollection<MotionZoneInfoDataGridModel>();
            VideoService = videoService;
            VideoCapture = videoService.VideoDevice.VideoCapture;
            this.CommandInit();
            this.MessageRegistration();
            Messenger.Default.Send<MotionZoneMessage>(new MotionZoneMessage()
            {
                VideoSource = videoService.VideoDevice.VideoCapture
            });


            for (int i = 0; i < videoService.VideoDevice.VideoCapture.MotionZones.Count; i++)
            {
                MotionZone zone = videoService.VideoDevice.VideoCapture.MotionZones.Item[i];
                string name = "Zone " + MotionZoneList.Count;
                MotionZoneList.Add(new MotionZoneInfoDataGridModel()
                {
                    Name = name,
                    Zone = zone,
                    Sensitivity = zone.Sensitivity

                });
            }

            SelectedMotionZone = MotionZoneList.Last();
        }

        private int GetIndex()
        {
            for (int i = 0; i < VideoCapture.MotionZones.Count; i++)
            {
                MotionZone zone = VideoCapture.MotionZones.Item[i];
                if (SelectedMotionZone.Zone.Equals(zone))
                {
                    return i;
                }
            }
            return -1;
        }


        #region Messages Registration

        private void MessageRegistration()
        {
            Messenger.Default.Register<MotionZoneMessage>(this, (message) =>
            {
                if (message.Zone != null && (message.ShowSelectedZone == null || message.ShowSelectedZone != true))
                {
                    var pom = GetIndex();
                    if (pom != -1)
                    {
                        var a = VideoCapture.MotionZones.get_Item(pom);
                        a = message.Zone;
                    }
                    SelectedMotionZone.Zone = message.Zone;

                }
            });
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
            if (win != null)
            {
                VideoService.VideoDevice = null;
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
                Sensitivity = zone.Sensitivity
            });

            VideoCapture.MotionZones.Add(zone);
            SelectedMotionZone = MotionZoneList.Last();
        }

        private bool CanDeleteMotionZone()
        {
            return MotionZoneList.Count != 0 && SelectedMotionZone != null;
        }

        private void DeleteMotionZone()
        {
            var pom = SelectedMotionZone;
            VideoCapture.MotionZones.Remove(pom.Zone);
            MotionZoneList.Remove(pom);
            if (MotionZoneList.Count != 0)
            {
                SelectedMotionZone = MotionZoneList.Last();
            }
        }
        #endregion
    }
}