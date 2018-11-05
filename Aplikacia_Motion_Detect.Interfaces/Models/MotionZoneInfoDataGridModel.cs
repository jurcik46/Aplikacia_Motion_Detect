using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DTKVideoCapLib;

namespace Aplikacia_Motion_Detect.Interfaces.Models
{
    public class MotionZoneInfoDataGridModel
    {

        private MotionZone _motionZone;
        private string _name;

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
