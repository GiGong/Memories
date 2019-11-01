using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Memories.Core.Controls
{
    public class MMRichTextBox : RichTextBox
    {
        #region Field

        private bool _isTextChanged;

        #endregion Field

        #region Dependency Property

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(byte[]), typeof(MMRichTextBox), new FrameworkPropertyMetadata(null, Data_Changed));

        public static readonly DependencyProperty GotKeyboardFocusCommandProperty =
            DependencyProperty.Register(nameof(GotKeyboardFocusCommand), typeof(ICommand), typeof(MMRichTextBox), new PropertyMetadata(null));

        #endregion Dependency Property

        #region Property

        public byte[] Data
        {
            get { return (byte[])GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public ICommand GotKeyboardFocusCommand
        {
            get { return (ICommand)GetValue(GotKeyboardFocusCommandProperty); }
            set { SetValue(GotKeyboardFocusCommandProperty, value); }
        }

        #endregion Property

        #region Constructor

        public MMRichTextBox()
        {
            _isTextChanged = false;

            IsInactiveSelectionHighlightEnabled = true;
        }

        #endregion Constructor

        #region Method

        private static void Data_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is MMRichTextBox richTextBox)
            {
                richTextBox.OnDataChanged();
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            _isTextChanged = true;
            Data = Save();
        }

        private void OnDataChanged()
        {
            if (!_isTextChanged)
            {
                _isTextChanged = false;
                Load(Data);
            }
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (GotKeyboardFocusCommand != null && GotKeyboardFocusCommand.CanExecute(this))
            {
                GotKeyboardFocusCommand.Execute(this);
            }
        }


        /// <summary>
        /// <see cref="https://docs.microsoft.com/ko-kr/dotnet/framework/wpf/controls/how-to-save-load-and-print-richtextbox-content"/>
        /// </summary>
        public byte[] Save()
        {
            byte[] buffer;
            using (MemoryStream mStream = new MemoryStream())
            {
                TextRange range = new TextRange(Document.ContentStart, Document.ContentEnd);
                range.Save(mStream, DataFormats.Rtf);
                buffer = mStream.ToArray();
            }
            return buffer;
        }

        public void Load(byte[] text)
        {
            TextRange range;
            using (MemoryStream mStream = new MemoryStream(text))
            {
                range = new TextRange(Document.ContentStart, Document.ContentEnd);
                range.Load(mStream, DataFormats.Rtf);
            }
        }

        #endregion Method
    }
}
