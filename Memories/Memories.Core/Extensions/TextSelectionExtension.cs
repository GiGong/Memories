using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Memories.Core.Extensions
{
    public static class TextSelectionExtension
    {
        /// <see cref="https://github.com/microsoft/WPF-Samples/tree/master/Sample%20Applications/FontDialog"/>
        public static double PointsToPixels(double value) => value * (96.0 / 72.0);
        public static double PixelsToPoints(double value) => value * (72.0 / 96.0);


        public static FontFamily GetFontFamily(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(TextElement.FontFamilyProperty);
            return property is FontFamily ? (FontFamily)property : new FontFamily();
        }

        public static void SetFontFamily(this TextSelection selection, FontFamily fontFamily)
        {
            selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily);
        }

        public static double? GetFontSize(this TextSelection selection)
        {
            double? nullValue = null;
            var property = selection.GetPropertyValue(TextElement.FontSizeProperty);
            return property is double ? PixelsToPoints((double)property) : nullValue;
        }

        public static void SetFontSize(this TextSelection selection, double fontSize)
        {
            selection.ApplyPropertyValue(TextElement.FontSizeProperty, PointsToPixels(fontSize));
        }

        public static FontWeight GetFontWeight(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(TextElement.FontWeightProperty);
            return property is FontWeight ? (FontWeight)property : FontWeights.Normal;
        }

        public static FontStyle GetFontStyle(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(TextElement.FontStyleProperty);
            return property is FontStyle ? (FontStyle)property : FontStyles.Normal;
        }

        public static TextDecorationCollection GetTextDecorations(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Inline.TextDecorationsProperty);
            return property is TextDecorationCollection ? (TextDecorationCollection)property : new TextDecorationCollection();
        }

        public static TextAlignment GetTextAlignment(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Block.TextAlignmentProperty);
            return property is TextAlignment ? (TextAlignment)property : TextAlignment.Left;
        }


        public static Brush GetForeground(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(TextElement.ForegroundProperty);
            return property is Brush ? (Brush)property : Brushes.Black;
        }

        public static void SetForeground(this TextSelection selection, Brush brush)
        {
            selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
        }

        public static Brush GetHighlight(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(TextElement.BackgroundProperty);
            return property is Brush ? (Brush)property : Brushes.Transparent;
        }

        public static void SetHighlight(this TextSelection selection, Brush brush)
        {
            selection.ApplyPropertyValue(TextElement.BackgroundProperty, brush);
        }
    }
}
