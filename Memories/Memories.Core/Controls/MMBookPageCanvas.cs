using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMBookPageCanvas : Canvas
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MMBookPageCanvas));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ClickCount == 1)
            {
                Keyboard.ClearFocus();
                var source = e.OriginalSource as FrameworkElement;

                if (source is Image && source.Parent is MMCenterImage image)
                {
                    if (image.Command?.CanExecute(image) ?? false)
                    {
                        image.Command.Execute(image);
                    }
                }
                else if (Command != null && Command.CanExecute(this))
                {
                    Command.Execute(this);
                }
            }
        }

    }
}
