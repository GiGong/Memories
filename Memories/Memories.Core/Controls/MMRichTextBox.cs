using Memories.Core.Formatter;
using System.Windows;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMRichTextBox : Xceed.Wpf.Toolkit.RichTextBox
    {

        public static readonly DependencyProperty GotKeyboardFocusCommandProperty =
            DependencyProperty.Register(nameof(GotKeyboardFocusCommand), typeof(ICommand), typeof(MMRichTextBox), new PropertyMetadata(null));

        public ICommand GotKeyboardFocusCommand
        {
            get { return (ICommand)GetValue(GotKeyboardFocusCommandProperty); }
            set { SetValue(GotKeyboardFocusCommandProperty, value); }
        }

        public MMRichTextBox()
        {
            TextFormatter = new RtfUTF8Formatter();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (GotKeyboardFocusCommand != null && GotKeyboardFocusCommand.CanExecute(this))
            {
                GotKeyboardFocusCommand.Execute(this);
            }
        }
    }
}
