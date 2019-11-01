using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Memories.Core.Extensions
{
    public static class TextSelectionExtension
    {
        public static FontFamily GetFontFamily(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Run.FontFamilyProperty);
            return property is FontFamily ? (FontFamily)property : new FontFamily();
        }

        public static void SetFontFamily(this TextSelection selection, FontFamily fontFamily)
        {
            selection.ApplyPropertyValue(Run.FontFamilyProperty, fontFamily);
        }

        public static double? GetFontSize(this TextSelection selection)
        {
            double? nullValue = null;
            var property = selection.GetPropertyValue(Run.FontSizeProperty);
            return property is double ? (double)property : nullValue;
        }

        public static void SetFontSize(this TextSelection selection, double fontSize)
        {
            selection.ApplyPropertyValue(Run.FontSizeProperty, fontSize);
        }

        public static FontWeight GetFontWeight(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Run.FontWeightProperty);
            return property is FontWeight ? (FontWeight)property : FontWeights.Normal;
        }

        public static FontStyle GetFontStyle(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Run.FontStyleProperty);
            return property is FontStyle ? (FontStyle)property : FontStyles.Normal;
        }

        public static TextDecorationCollection GetTextDecorations(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Run.TextDecorationsProperty);
            return property is TextDecorationCollection ? (TextDecorationCollection)property : new TextDecorationCollection();
        }

        public static TextAlignment GetTextAlignment(this TextSelection selection)
        {
            var property = selection.GetPropertyValue(Paragraph.TextAlignmentProperty);
            return property is TextAlignment ? (TextAlignment)property : TextAlignment.Left;
        }
    }
}
