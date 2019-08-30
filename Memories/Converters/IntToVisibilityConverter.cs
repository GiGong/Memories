using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Memories.Converters
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int) && !int.TryParse(value.ToString(), out _))
            {
                throw new ApplicationException("Value has to be of type '" + typeof(int).FullName + "'!");
            }

            if (!(parameter is int) && !int.TryParse(parameter.ToString(), out _))
            {
                throw new ApplicationException("Parameter has to be of type '" + typeof(int).FullName + "'!");
            }

            int intParam = System.Convert.ToInt32(parameter);
            int val = System.Convert.ToInt32(value);

            return ((intParam & val) != 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("This converter can only convert!");
        }
    }
}
