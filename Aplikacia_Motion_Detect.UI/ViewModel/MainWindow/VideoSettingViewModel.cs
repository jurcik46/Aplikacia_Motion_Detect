using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Aplikacia_Motion_Detect.UI.ViewModel.MainWindow
{
    public class VideoSettingViewModel : ViewModelBase
    {
        private string _name;
        private string _description;
        private string _state;
        private string _lastError;
        private string _type;
        private string _reesolution;
        private string _pixelFormat;
        private string _frames;
        private string _fps;
        private string _active;

        public VideoSettingViewModel()
        {

        }

        public string Active { get => _active; set => _active = value; }
        public string Fps { get => _fps; set => _fps = value; }
        public string Frames { get => _frames; set => _frames = value; }
        public string PixelFormat { get => _pixelFormat; set => _pixelFormat = value; }
        public string Reesolution { get => _reesolution; set => _reesolution = value; }
        public string Type { get => _type; set => _type = value; }
        public string LastError { get => _lastError; set => _lastError = value; }
        public string State { get => _state; set => _state = value; }
        public string Description { get => _description; set => _description = value; }
        public string Name { get => _name; set => _name = value; }
    }
}
