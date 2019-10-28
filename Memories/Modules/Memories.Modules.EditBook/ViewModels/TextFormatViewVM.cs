using Memories.Core.Controls;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Prism.Events;
using Prism.Mvvm;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class TextFormatViewVM : BindableBase
    {
        private bool _isBold;
        private bool _isItalic;
        private bool _isUnderline;
        private TextAlignment _alignType;

        private MMRichTextBox _richTextBox;

        private readonly IEventAggregator _eventAggregator;

        #region Property

        public bool IsBold
        {
            get { return _isBold; }
            set { SetProperty(ref _isBold, value); }
        }

        public bool IsItalic
        {
            get { return _isItalic; }
            set { SetProperty(ref _isItalic, value); }
        }

        public bool IsUnderline
        {
            get { return _isUnderline; }
            set { SetProperty(ref _isUnderline, value); }
        }

        public TextAlignment AlignType
        {
            get { return _alignType; }
            set { SetProperty(ref _alignType, value); }
        }

        public MMRichTextBox RichTextBox
        {
            get { return _richTextBox; }
            set { SetProperty(ref _richTextBox, value); }
        }

        #endregion Property

        #region Constructor

        public TextFormatViewVM(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Subscribe(RichTextBoxSelected);
        }

        #endregion Constructor

        #region Method

        private void RichTextBoxSelected(MMRichTextBox newRichTextBox)
        {
            RichTextBox = newRichTextBox;

            if (RichTextBox == null)
            {
                ClearFormats();
            }
            else
            {
                RichTextBox.SelectionChanged += RichTextBox_SelectionChanged;
            }
        }

        private void ClearFormats()
        {
            IsBold = false;
            IsItalic = false;
            IsUnderline = false;

            AlignType = TextAlignment.Left;
        }

        private void UpdateFormats()
        {
            var selection = RichTextBox.Selection;
            IsBold = selection.GetFontWeight() > FontWeights.Normal;
            IsItalic = selection.GetFontStyle() == FontStyles.Italic;
            IsUnderline = selection.GetTextDecorations() == TextDecorations.Underline;

            AlignType = selection.GetTextAlignment();
        }


        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateFormats();
        }

        #endregion Method
    }
}
