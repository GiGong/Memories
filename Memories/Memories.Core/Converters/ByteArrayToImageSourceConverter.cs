using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Core.Converters
{
    [ValueConversion(typeof(byte[]), typeof(BitmapSource))]
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : (BitmapSource)new ImageSourceConverter().ConvertFrom(value as byte[]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return SourceToByteArray(value as BitmapSource);
        }

        public static byte[] SourceToByteArray(BitmapSource bitmap)
        {
            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
    }
}
