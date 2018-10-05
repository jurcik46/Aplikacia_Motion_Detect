using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplikacia_Motion_Detect.UI.Views.AddVideoDevice;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Aplikacia_Motion_Detect.UI.ViewModels.MainWindow
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
        private VideoCaptureWindow VideoCaptureWindow;
        public List<Class1> B { get; set; }
        private RelayCommand _addVideo;

        public VideoSettingViewModel()
        {
            this.CommandInit();
            this.MessageRegister();

            B = new List<Class1>();
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));
            B.Add(new Class1("test", "test"));




        }

        private void MessageRegister()
        {

        }

        private void CommandInit()
        {
            this.AddVideo = new RelayCommand(this.ShowVideoCaptureWindow, this.CanShowVideoCaptureWindow);
        }


        public bool CanShowVideoCaptureWindow()
        {
            if (this.VideoCaptureWindow != null)
                return (this.VideoCaptureWindow.IsLoaded) ? false : true;
            else
                return true;
        }

        public void ShowVideoCaptureWindow()
        {
            this.VideoCaptureWindow = new VideoCaptureWindow();
            this.VideoCaptureWindow.Show();
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
        public RelayCommand AddVideo { get => _addVideo; set => _addVideo = value; }
    }
}
