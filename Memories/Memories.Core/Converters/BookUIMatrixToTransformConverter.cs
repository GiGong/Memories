using Memories.Business.Models;
using Memories.Core.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Memories.Core.Converters
{
    [ValueConversion(typeof(BookUIMatrix), typeof(Transform))]
    public class BookUIMatrixToTransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new MatrixTransform(((BookUIMatrix)value).ToMatrix());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Matrix)value).ToBookUIMatrix();
        }
    }
}
