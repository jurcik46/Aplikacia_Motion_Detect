using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;
using System.Windows.Threading;

namespace Aplikacia_Motion_Detect.Interfaces.Models
{
    public class MotionZoneInfoDataGridModel
    {

        private MotionZone _motionZone;
        private string _name;
        private int _timer;
        private int _number;
        private DispatcherTimer _dispatcherTimer;
        public MotionZone Zone
        {
            get { return _motionZone; }
            set
            {
                _motionZone = value;
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

        public int Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                OnPropertyChanged();
            }
        }

        public int Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        public DispatcherTimer DispatcherTimer
        {
            get { return _dispatcherTimer; }
            set { _dispatcherTimer = value; }
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
