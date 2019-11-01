using System;
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

        public TextBox SearchTextBox { get; }
        public Button ViewListBoxButton { get;}
        public ListBox FontFamilyListBox { get; }


        public FontFamily SelectedFontFamily
        {
            get => (FontFamily)GetValue(SelectedFontFamilyProperty);
            set
            {
                SetValue(SelectedFontFamilyProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontFamily)));
            }
        }

        private void SetTextToItem() => SearchTextBox.Text = FontFamilyListBox.SelectedItem.ToString() ?? SearchTextBox.Text;


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
                BorderThickness = new Thickness(0),
                DataContext = this
            };
            SearchTextBox.TextChanged += TextBox_TextChanged;
            SearchTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            SearchTextBox.LostKeyboardFocus += This_LostFocus;

            ViewListBoxButton = new Button()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Content = "▽"
            };
            ViewListBoxButton.Click += Button_Click;

            FontFamilyListBox = new ListBox()
            {
                ItemsSource = _listFontFamily,
                Visibility = Visibility.Collapsed,
                SelectionMode = SelectionMode.Single,
                VerticalAlignment = VerticalAlignment.Top,
                DataContext = this
            };
            FontFamilyListBox.SetBinding(ListBox.SelectedItemProperty, new Binding(nameof(SelectedFontFamily)) { Mode = BindingMode.TwoWay });
            FontFamilyListBox.KeyDown += ListBox_KeyDown;
            FontFamilyListBox.MouseLeftButtonDown += ListBox_MouseLeftButtonDown;
            FontFamilyListBox.LostKeyboardFocus += This_LostFocus;
            FontFamilyListBox.SelectionChanged += ListBox_SelectionChanged;

            SizeChanged += This_SizeChanged;
            LostKeyboardFocus += This_LostFocus;

            Children.Add(SearchTextBox);
            Children.Add(ViewListBoxButton);
            Children.Add(FontFamilyListBox);
        }

        private void This_LostFocus(object sender, RoutedEventArgs e)
        {
            return;
            //FontFamilyListBox.Visibility = Visibility.Collapsed;
            //SearchTextBox.Text = FontFamilyListBox.SelectedItem?.ToString() ?? string.Empty;
        }

        private void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SearchTextBox.Height = ActualHeight;
            FontFamilyListBox.Margin = new Thickness(0, ActualHeight, 0, -ActualHeight - ListBox_Height);
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetTextToItem();
            }

            SearchTextBox.CaretIndex = SearchTextBox.Text.Length;
            SearchTextBox.Focus();
        }

        private void ListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            return;
            //if (ItemsControl.ContainerFromElement(FontFamilyListBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            //{
            //    SearchTextBox.TextChanged -= TextBox_TextChanged;
            //    SearchTextBox.Text = (string)item.Content;
            //    SearchTextBox.TextChanged += TextBox_TextChanged;
            //}
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.TextChanged -= TextBox_TextChanged;
            SearchTextBox.Text = FontFamilyListBox.SelectedItem.ToString();
            SearchTextBox.TextChanged += TextBox_TextChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FontFamilyListBox.Visibility == Visibility.Visible)
            {
                FontFamilyListBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                FontFamilyListBox.Visibility = Visibility.Visible;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (!FontFamilyListBox.Items.MoveCurrentToNext())
                    {
                        FontFamilyListBox.Items.MoveCurrentToLast();
                        e.Handled = true;
                    }
                    break;

                case Key.Down:
                    if (!FontFamilyListBox.Items.MoveCurrentToPrevious())
                    {
                        FontFamilyListBox.Items.MoveCurrentToFirst();
                        e.Handled = true;
                    }
                    break;

                case Key.Enter:
                    SetTextToItem();
                    SearchTextBox.CaretIndex = SearchTextBox.Text.Length;
                    e.Handled = true;
                    return;

                case Key.Escape:
                    Keyboard.ClearFocus();
                    return;

                default:
                    return;
            }

            ListBoxItem lbi = (ListBoxItem)FontFamilyListBox.ItemContainerGenerator.ContainerFromItem(FontFamilyListBox.SelectedItem);
            lbi?.Focus();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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