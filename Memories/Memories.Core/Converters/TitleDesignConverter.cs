using System;
using System.Globalization;
using System.Windows.Data;

namespace Memories.Core.Converters
{
    public class TitleDesignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value.ToString();
            return input.Length > 1 ? input.Remove(0, 1) : input;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
