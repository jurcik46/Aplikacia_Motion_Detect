using System.Collections.ObjectModel;
using System.Linq;
using Aplikacia_Motion_Detect.Interfaces.Enums;
using Aplikacia_Motion_Detect.Interfaces.Extensions;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Models;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Serilog;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MotionZones
{
    public class MotionZonesViewModel : ViewModelBase
    {
        public ILogger Logger => Log.Logger.ForContext<MotionZonesViewModel>();

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

            Logger.Debug(MotionZonesViewModelEvents.Create, "Creating new instance of MotionZonesViewModel");

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
                    Number = MotionZoneList.Count,
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
                Logger.Debug(MotionZonesViewModelEvents.SetSelectToLast);
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
            Logger.Debug(MotionZonesViewModelEvents.OkCommand);
            var pomVideoZones = VideoService.FoundEqualsVideoCapture(VideoDevice.VideoCapture);
            pomVideoZones.MotionZones = new System.Collections.Generic.List<MotionZoneInfoDataGridModel>();
            foreach (var pomZone in MotionZoneList)
            {
                pomVideoZones.MotionZones.Add(pomZone);
            }
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
                Number = MotionZoneList.Count,
                Name = name,
                Zone = zone,
                Timer = 0,
            });
            Logger.Information(MotionZonesViewModelEvents.AddCommand, "Name of zone {name} number {number} \n{@zone}", name, MotionZoneList.Count, zone);
            VideoDevice.VideoCapture.MotionZones.Add(zone);
            SetSelectToLast();
        }

        private bool CanDeleteMotionZone()
        {
            return MotionZoneList.Count != 0 && SelectedMotionZone != null;
        }

        private void DeleteMotionZone()
        {
            Logger.Information(MotionZonesViewModelEvents.DeleteCommand, "Name of zone {name} \n{@zone}", SelectedMotionZone.Name, SelectedMotionZone.Zone);
            VideoDevice.VideoCapture.MotionZones.Remove(SelectedMotionZone.Zone);
            MotionZoneList.Remove(SelectedMotionZone);
            SetSelectToLast();

        }
        #endregion
    }
}