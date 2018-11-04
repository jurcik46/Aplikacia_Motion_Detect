using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Aplikacia_Motion_Detect.Interfaces.Convertors
{
    [ValueConversion(typeof(bool), typeof(GridLength))]
    public class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            if (!(value is bool))
            {
                return null;
            }
            var boolValue = (bool)value;
            return boolValue ? "True" : "False";
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            if (!(value is string))
            {
                return null;
            }

            var boolValue = (string)value;
            if (boolValue.Equals("False"))
            {
                return false;
            }
            if (boolValue.Equals("True"))
            {
                return true;
            }


            return null;

        }
    }
}
