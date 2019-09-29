using Memories.Business.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookViewVM : BindableBase
    {
        #region Field

        private Book _editBook;
        private BookPage _leftPage;
        private BookPage _rightPage;

        private int _leftNum;
        private int _rightNum;
        private int _totalNum;

        private DelegateCommand _pageBackCommand;
        private DelegateCommand _pageForwardCommand;

        #endregion Field

        #region Property

        public Book EditBook
        {
            get { return _editBook; }
            set
            {
                SetProperty(ref _editBook, value);

                if (EditBook == null)
                {
                    TotalNum = 0;
                    LeftNum = 0;
                    RightNum = 0;
                }
                else
                {
                    TotalNum = EditBook.BookPages.Count;
                    LeftNum = 1;
                    RightNum = 2;
                }
            }
        }

        public BookPage LeftPage
        {
            get { return _leftPage; }
            set { SetProperty(ref _leftPage, value); }
        }

        public BookPage RightPage
        {
            get { return _rightPage; }
            set { SetProperty(ref _rightPage, value); }
        }

        public int LeftNum
        {
            get { return _leftNum; }
            set
            {
                SetProperty(ref _leftNum, value);

                LeftPage = EditBook.BookPages[LeftNum - 1];
            }
        }

        public int RightNum
        {
            get { return _rightNum; }
            set
            {
                SetProperty(ref _rightNum, value);

                RightPage = RightNum > TotalNum ? null : EditBook.BookPages[RightNum - 1];
            }
        }

        public int TotalNum
        {
            get { return _totalNum; }
            set { SetProperty(ref _totalNum, value); }
        }

        #endregion Property

        #region Constructor

        public BookViewVM()
        {
        }

        #endregion Constructor

        #region Command

        public DelegateCommand PageBackCommand =>
            _pageBackCommand ?? (_pageBackCommand = new DelegateCommand(PageBack, CanPageBack).ObservesProperty(() => LeftNum));

        public DelegateCommand PageForwardCommand =>
            _pageForwardCommand ?? (_pageForwardCommand = new DelegateCommand(PageForward, CanPageForward).ObservesProperty(() => RightNum));

        #endregion Command

        #region Method

        void PageBack()
        {
            LeftNum -= 2;
            RightNum -= 2;
        }

        bool CanPageBack()
        {
            return LeftNum > 1;
        }

        void PageForward()
        {
            LeftNum += 2;
            RightNum += 2;
        }

        bool CanPageForward()
        {
            return RightNum < TotalNum;
        }

        #endregion Method
    }
}
