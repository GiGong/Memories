using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMBookPageCanvas : Canvas
    {
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(MMBookPageCanvas));

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            Keyboard.ClearFocus();
            if (e.ClickCount == 2 && DoubleClickCommand != null)
            {
                var source = e.OriginalSource as FrameworkElement;

                if (source is Image && source.Parent is MMCenterImage image)
                {
                    source = image;
                }

                if (DoubleClickCommand.CanExecute(source))
                {
                    DoubleClickCommand.Execute(source);
                }
            }
        }

    }
}
