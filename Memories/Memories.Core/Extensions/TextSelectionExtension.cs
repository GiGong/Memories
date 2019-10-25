using System.Windows;
using System.Windows.Documents;

namespace Memories.Core.Extensions
{
    public static class TextSelectionExtension
    {
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
