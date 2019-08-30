using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Memories.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        private int val;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                throw new ApplicationException("Value has to be of type '" + typeof(int).FullName + "'!");
            }

            int intParam = (int)parameter;
            val = (int)value;

            return ((intParam & val) != 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
