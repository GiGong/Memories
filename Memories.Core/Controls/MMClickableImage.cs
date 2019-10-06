using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMClickableImage : Image
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(MMClickableImage));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 2 && Command != null)
            {
                if (Command.CanExecute(this))
                {
                    Command.Execute(this);
                    e.Handled = true;
                }
            }
        }
    }
}
