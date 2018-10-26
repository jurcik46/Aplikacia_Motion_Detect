using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Service;
using DTKVideoCapLib;
using GalaSoft.MvvmLight.Command;

namespace Aplikacia_Motion_Detect.UI.ViewModels.DeveloperKey
{
    public class DeveloperKeyViewModel
    {
        private IVideoService _vidoeService;


        private RelayCommand<IClosable> _saveKeyCommand;

        public string DevKey { get; set; }
        public RelayCommand<IClosable> SaveKeyCommand
        {
            get { return _saveKeyCommand; }
            set { _saveKeyCommand = value; }
        }

        public IVideoService VidoeService
        {
            get { return _vidoeService; }
            set { _vidoeService = value; }
        }

        public DeveloperKeyViewModel(IVideoService videoService)
        {
            VidoeService = videoService;
            DevKey = VidoeService.DeveloperKey;
            this.CommandInit();
        }

        private void CommandInit()
        {

            this.SaveKeyCommand = new RelayCommand<IClosable>(this.Save, this.CanSave);
        }

        private bool CanSave(IClosable win)
        {
            return true;
        }

        private void Save(IClosable win)
        {
            VidoeService.DeveloperKey = DevKey;
            VidoeService.SaveConfig();
            //if (win != null)
            //{
            //    win.Close();
            //}
        }


    }
}
