using Memories.Core.Controls;
using Memories.Core.Events;
using Memories.Core.Extensions;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Memories.Modules.EditBook.ViewModels
{
    public class TextFormatViewVM : BindableBase
    {
        #region Field

        private bool _isUpdate;

        private string _fontSizeText;
        private string _selectedFontFamily;

        private bool _isBold;
        private bool _isItalic;
        private bool _isUnderline;
        private TextAlignment? _alignType;

        private MMRichTextBox _richTextBox;
        private bool _isRichTextBox;

        #endregion Field

        #region Property

        public static double[] FontSizes
        {
            get
            {
                return new double[] {
                    3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5,
                    10.0, 10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 15.0,
                    16.0, 17.0, 18.0, 19.0, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0,
                    32.0, 34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0, 68.0, 72.0, 76.0,
                    80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
                    };
            }
        }

        public string[] FontFamilies { get; }

        public string FontSizeText
        {
            get { return _fontSizeText; }
            set
            {
                SetProperty(ref _fontSizeText, value);
                if (!_isUpdate && double.TryParse(FontSizeText, out double fontSize))
                {
                    RichTextBox?.Selection.SetFontSize(fontSize);
                }
            }
        }

        public string SelectedFontFamily
        {
            get { return _selectedFontFamily; }
            set
            {
                SetProperty(ref _selectedFontFamily, value);
                if (!_isUpdate && SelectedFontFamily != null)
                {
                    RichTextBox.Selection.SetFontFamily(new FontFamily(SelectedFontFamily));
                }
            }
        }

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

        public TextAlignment? AlignType
        {
            get { return _alignType; }
            set { SetProperty(ref _alignType, value); }
        }

        public MMRichTextBox RichTextBox
        {
            get { return _richTextBox; }
            set { SetProperty(ref _richTextBox, value); }
        }

        public bool IsRichTextBox
        {
            get { return _isRichTextBox; }
            set { SetProperty(ref _isRichTextBox, value); }
        }

        #endregion Property

        #region Constructor

        public TextFormatViewVM(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<RichTextBoxSelectedEvent>().Subscribe(RichTextBoxSelected);

            _isUpdate = false;

            var cond = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name);
            var list = Fonts.SystemFontFamilies.Select(
                font =>
                {
                    if (font.FamilyNames.ContainsKey(cond))
                        return font.FamilyNames[cond];
                    else
                        return font.ToString();
                }).ToList();
            list.Insert(0, string.Empty);
            list.Sort();
            FontFamilies = list.ToArray();
        }

        #endregion Constructor

        #region Method

        private void RichTextBoxSelected(MMRichTextBox newRichTextBox)
        {
            if (newRichTextBox == null)
            {
                ClearFormats();
                if (RichTextBox != null)
                {
                    RichTextBox.SelectionChanged -= RichTextBox_SelectionChanged;
                }
                RichTextBox = newRichTextBox;
                IsRichTextBox = false;
            }
            else
            {
                RichTextBox = newRichTextBox;
                IsRichTextBox = true;
                RichTextBox.SelectionChanged += RichTextBox_SelectionChanged;
            }
        }

        private void ClearFormats()
        {
            IsBold = false;
            IsItalic = false;
            IsUnderline = false;

            AlignType = null;
        }

        private void UpdateFormats()
        {
            var selection = RichTextBox.Selection;
            _isUpdate = true;

            SelectedFontFamily = selection.GetFontFamily().ToString();
            FontSizeText = selection.GetFontSize().ToString();

            IsBold = selection.GetFontWeight() > FontWeights.Normal;
            IsItalic = selection.GetFontStyle() == FontStyles.Italic;
            IsUnderline = selection.GetTextDecorations() == TextDecorations.Underline;

            AlignType = selection.GetTextAlignment();

            _isUpdate = false;
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateFormats();
        }

        #endregion Method
    }
}
