using System.Windows.Controls;
using System.Windows.Input;

namespace Memories.Modules.EditBook.Views
{
    /// <summary>
    /// Interaction logic for TextFormatView
    /// </summary>
    public partial class TextFormatView : UserControl
    {
        private static readonly System.Globalization.NumberStyles _style = System.Globalization.NumberStyles.AllowDecimalPoint;
        private static readonly System.Globalization.CultureInfo _culture = System.Globalization.CultureInfo.InvariantCulture;

        public TextFormatView()
        {
            InitializeComponent();
        }

        private void FontSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Note: if TryParse has performance issue, change to regex
            //private static readonly Regex _regex = new Regex("[^0-9.]+");
            //e.Handled = _regex.IsMatch(e.Text);

            e.Handled = !double.TryParse(((ComboBox)sender).Text + e.Text, _style, _culture, out _);
        }
    }
}
