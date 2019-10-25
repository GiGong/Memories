using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Memories.Core.Controls
{
    public class MMFontFamilySearch : Grid, INotifyPropertyChanged
    {
        private readonly List<string> _listFontFamily;
        private double _listBox_Height;

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty SelectedFontFamilyProperty = DependencyProperty.Register("SelectedItem", typeof(FontFamily), typeof(MMFontFamilySearch), new PropertyMetadata());


        public double ListBox_Height
        {
            get => _listBox_Height;
            set
            {
                _listBox_Height = value;
                FontFamilyListBox.Height = ListBox_Height;
            }
        }

        public TextBox SearchTextBox { get; set; }
        public ListBox FontFamilyListBox { get; set; }


        public FontFamily SelectedFontFamily
        {
            get => (FontFamily)GetValue(SelectedFontFamilyProperty);
            set
            {
                SetValue(SelectedFontFamilyProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontFamily)));
            }
        }

        private void SetTextToItem() => SearchTextBox.Text = (string)FontFamilyListBox.SelectedItem ?? SearchTextBox.Text;


        public MMFontFamilySearch()
        {
            _listFontFamily = new List<string>(Fonts.SystemFontFamilies.Count);
            var cond = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name);
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                if (font.FamilyNames.ContainsKey(cond))
                    _listFontFamily.Add(font.FamilyNames[cond]);
                else
                    _listFontFamily.Add(font.ToString());
            }
            _listFontFamily.Sort();


            SearchTextBox = new TextBox()
            {
                VerticalAlignment = VerticalAlignment.Top,
                DataContext = this
            };
            SearchTextBox.TextChanged += SearchTextBox_TextChanged;


            FontFamilyListBox = new ListBox()
            {
                ItemsSource = _listFontFamily,
                Visibility = Visibility.Collapsed,
                SelectionMode = SelectionMode.Single,
                VerticalAlignment = VerticalAlignment.Top,
                DataContext = this
            };
            FontFamilyListBox.SetBinding(ListBox.SelectedItemProperty, new Binding(nameof(SelectedFontFamily)) { Mode = BindingMode.TwoWay });


            SearchTextBox.PreviewKeyDown += SearchTextBox_KeyDown;
            SearchTextBox.GotFocus += FontFamilySearchBox_GotFocus;
            SearchTextBox.LostFocus += FontFamilySearchBox_LostFocus;

            FontFamilyListBox.KeyDown += FontFamilyListBox_KeyDown;
            FontFamilyListBox.PreviewMouseLeftButtonDown += FontFamilyListBox_PreviewMouseLeftButtonDown;
            FontFamilyListBox.GotFocus += FontFamilySearchBox_GotFocus;
            FontFamilyListBox.LostFocus += FontFamilySearchBox_LostFocus;

            SizeChanged += FontFamilySearchBox_SizeChanged;
            GotFocus += FontFamilySearchBox_GotFocus;
            LostFocus += FontFamilySearchBox_LostFocus;

            Children.Add(SearchTextBox);
            Children.Add(FontFamilyListBox);
        }

        private void FontFamilySearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FontFamilyListBox.Visibility = Visibility.Visible;
        }

        private void FontFamilySearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsFocused == false
                && SearchTextBox.IsFocused == false
                && FontFamilyListBox.IsFocused == false)
            {
                FontFamilyListBox.Visibility = Visibility.Collapsed;
                SearchTextBox.Text = FontFamilyListBox.SelectedItem?.ToString() ?? string.Empty;
            }
        }

        private void FontFamilySearchBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SearchTextBox.Height = ActualHeight;
            FontFamilyListBox.Margin = new Thickness(0, ActualHeight, 0, -ActualHeight - ListBox_Height);
        }

        private void FontFamilyListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetTextToItem();
            }

            SearchTextBox.CaretIndex = SearchTextBox.Text.Length;
            SearchTextBox.Focus();
        }

        private void FontFamilyListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ContainerFromElement(FontFamilyListBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            {
                SearchTextBox.TextChanged -= SearchTextBox_TextChanged;
                SearchTextBox.Text = (string)item.Content;
                SearchTextBox.TextChanged += SearchTextBox_TextChanged;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (!FontFamilyListBox.Items.MoveCurrentToNext())
                    {
                        FontFamilyListBox.Items.MoveCurrentToLast();
                    }
                    break;

                case Key.Down:
                    if (!FontFamilyListBox.Items.MoveCurrentToPrevious())
                    {
                        FontFamilyListBox.Items.MoveCurrentToFirst();
                    }
                    break;

                case Key.Enter:
                    SetTextToItem();
                    SearchTextBox.CaretIndex = SearchTextBox.Text.Length;
                    return;

                case Key.Escape:
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    return;

                default:
                    return;
            }

            ListBoxItem lbi = (ListBoxItem)FontFamilyListBox.ItemContainerGenerator.ContainerFromItem(FontFamilyListBox.SelectedItem);
            lbi?.Focus();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                FontFamilyListBox.Visibility = Visibility.Collapsed;
                return;
            }

            FontFamilyListBox.Visibility = Visibility.Visible;

            string lower = SearchTextBox.Text.ToLower();
            foreach (var item in _listFontFamily)
            {
                if (item.ToLower().StartsWith(lower))
                {
                    FontFamilyListBox.SelectedItem = item;

                    ScrollViewer scrollViewer = GetScrollViewer(FontFamilyListBox) as ScrollViewer;
                    scrollViewer?.ScrollToHome();
                    FontFamilyListBox.ScrollIntoView(item);
                    return;
                }
            }
        }

        private static DependencyObject GetScrollViewer(DependencyObject o)
        {
            if (o is ScrollViewer)
            { return o; }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }

            return null;
        }
    }
}