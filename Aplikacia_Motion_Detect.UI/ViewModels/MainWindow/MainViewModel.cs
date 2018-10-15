using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Aplikacia_Motion_Detect.Interfaces.Interface.Services;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using DTKVideoCapLib;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        public IVideoService VideoService { get; private set; }

        #region Sub ViewModels
        public VideoSettingViewModel VideoSettingControl { get; set; }
        public VideoViewModel VideoControl { get; set; }

        #endregion
        public MainViewModel(IVideoService videoService)
        {
            VideoService = videoService;
            VideoControl = new VideoViewModel(videoService);
            VideoSettingControl = new VideoSettingViewModel(videoService);
            this.MessageRegister();
        }


        #region Messages

        private void MessageRegister()
        {
            Messenger.Default.Register<AddVideoCapture>(this, (message) =>
            {
                VideoSettingControl.SaveVideoCapture(message.CreateNew);


            });
        }
        #endregion
    }
}
