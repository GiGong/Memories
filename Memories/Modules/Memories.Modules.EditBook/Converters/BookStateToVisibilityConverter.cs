using Memories.Modules.EditBook.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Memories.Modules.EditBook.Converters
{
    public class BookStateToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is BookState && parameter is BookState))
            {
                throw new ApplicationException("Parameter Error in " + nameof(BookStateToVisibilityConverter));
            }

            BookState valueEnum = (BookState)value;
            BookState parameterEnum = (BookState)parameter;

            return valueEnum.Equals(parameterEnum) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
