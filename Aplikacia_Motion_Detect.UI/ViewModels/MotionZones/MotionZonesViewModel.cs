using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MotionZones
{
    public class MotionZonesViewModel : ViewModelBase
    {

        #region Command Declaration

        private RelayCommand<IClosable> _okCommand;

        public RelayCommand<IClosable> OkCommand
        {
            get { return _okCommand; }
            set { _okCommand = value; }
        }

        #endregion

        private IVideoService VideoService { get; set; }


        public MotionZonesViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            this.CommandInit();
        }



        #region Commands
        private void CommandInit()
        {
            this.OkCommand = new RelayCommand<IClosable>(this.ExecuteOk);
        }

        public void ExecuteOk(IClosable win)
        {
            if (win != null)
            {
                win.Close();
            }
        }
        #endregion
    }
}
