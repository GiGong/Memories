using System.Windows;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMRichTextBox : Xceed.Wpf.Toolkit.RichTextBox
    {

        public static readonly DependencyProperty GotFocusCommandProperty =
            DependencyProperty.Register("GotFocusCommand", typeof(ICommand), typeof(MMRichTextBox), new PropertyMetadata(null));

        public ICommand GotFocusCommand
        {
            get { return (ICommand)GetValue(GotFocusCommandProperty); }
            set { SetValue(GotFocusCommandProperty, value); }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (GotFocusCommand != null && GotFocusCommand.CanExecute(this))
            {
                GotFocusCommand.Execute(this);
            }
        }
    }
}
