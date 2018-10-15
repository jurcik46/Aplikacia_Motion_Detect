using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using GalaSoft.MvvmLight;

namespace Aplikacia_Motion_Detect.UI.ViewModels.AddVideoDevice
{
    public class SaveOutputViewModel : ViewModelBase
    {
        private IVideoService VideoService { get; set; }

        public SaveOutputViewModel(IVideoService videoService)
        {
            VideoService = videoService;
        }
    }
}
