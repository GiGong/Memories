using Memories.Business.Models;
using Memories.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookPageViewVM : BindableBase
    {
        private ObservableCollection<UIElement> _pageControls;
        public ObservableCollection<UIElement> PageControls
        {
            get { return _pageControls; }
            set { SetProperty(ref _pageControls, value); }
        }

        private BookPage _nowPage;
        public BookPage NowPage
        {
            get { return _nowPage; }
            set
            {
                SetProperty(ref _nowPage, value);


                PageControls = NowPage == null ? null : new ObservableCollection<UIElement>(NowPage.PageControls.Select<BookUI, UIElement>(
                    (s) =>
                    {
                        if (s.UIType == Business.Enums.BookUIEnum.TextUI)
                        {
                            return (s as BookTextUI).ToRichTextBox();
                        }
                        else if (s.UIType == Business.Enums.BookUIEnum.ImageUI)
                        {
                            return (s as BookImageUI).ToImage();
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(s + " is not BookUI");
                        }
                    }));
            }
        }

        public BookPageViewVM()
        {

        }
    }
}
