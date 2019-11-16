using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Memories.Core.Controls
{
    public class MMListView : ListView
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MMListView), new PropertyMetadata(null));

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(MMListView), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        public MMListView()
        {
            Style itemStyle = new Style(typeof(ListViewItem));
            EventSetter eventSetter = new EventSetter(MouseDoubleClickEvent, new MouseButtonEventHandler(ItemMouseDoubleClick));
            itemStyle.Setters.Add(eventSetter);
            ItemContainerStyle = itemStyle;
        }

        private void ItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ListViewItem item))
            {
                return;
            }

            if (DoubleClickCommand != null && DoubleClickCommand.CanExecute(item))
            {
                DoubleClickCommand.Execute(item);
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            foreach (ScrollBar scroll in FindVisualChildren<ScrollBar>(this))
            {
                if (scroll.Orientation == Orientation.Vertical
                    && scroll.Value == scroll.Maximum)
                {
                    if (Command != null && Command.CanExecute(this))
                    {
                        Command.Execute(this);
                    }
                }
            }

            base.OnPreviewMouseWheel(e);
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
           where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
