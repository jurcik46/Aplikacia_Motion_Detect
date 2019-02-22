using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aplikacia_Motion_Detect.Interfaces.Annotations;
using Aplikacia_Motion_Detect.Interfaces.Messages;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Models
{
    public class VideoInfoDataGridModel : INotifyPropertyChanged
    {

        private string _name;
        private string _description;
        private string _state;
        private string _lastError;
        private string _type;
        private string _resolution;
        private string _pixel;
        private int _frames;
        private int _fps;
        private bool _enable = true;
        private VideoCapture _videoCapture;
        private List<MotionZoneInfoDataGridModel> _motionZones;

        public List<MotionZoneInfoDataGridModel> MotionZones { get; set; }

        [CanBeNull] private Queue<int> _frameTick;

        public VideoCapture VideoCapture
        {
            get { return _videoCapture; }
            set
            {
                _videoCapture = value;
                OnPropertyChanged();
            }
        }

        public int FPS
        {
            get { return _fps; }
            set
            {
                _fps = value;
                OnPropertyChanged();
            }
        }

        public int Frames
        {
            get { return _frames; }
            set
            {
                _frames = value;
                OnPropertyChanged();
            }
        }

        public string Pixel
        {
            get { return _pixel; }
            set
            {
                _pixel = value;
                OnPropertyChanged();
            }
        }

        public string Resolution
        {
            get { return _resolution; }
            set
            {
                _resolution = value;
                OnPropertyChanged();
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public string LastError
        {
            get { return _lastError; }
            set
            {
                _lastError = value;
                OnPropertyChanged();
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public Queue<int> FrameTick
        {
            get { return _frameTick; }
            set
            {
                _frameTick = value;
                OnPropertyChanged();
            }
        }

        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChanged Impl
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
