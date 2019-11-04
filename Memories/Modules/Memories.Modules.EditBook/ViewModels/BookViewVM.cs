using Memories.Business.Models;
using Memories.Core.Extensions;
using Memories.Modules.EditBook.Enums;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Specialized;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookViewVM : BindableBase
    {
        #region Field

        private Book _editBook;

        private double _paperWidth;
        private double _paperHeight;

        private BookPage _leftPage;
        private BookPage _rightPage;

        private int _leftNum;
        private int _rightNum;
        private int _totalNum;

        private bool _isRightExist;

        private BookState _bookState;

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
                    EditBookChanged();
                }
            }
        }

        public double PaperWidth
        {
            get { return _paperWidth; }
            set { SetProperty(ref _paperWidth, value); }
        }

        public double PaperHeight
        {
            get { return _paperHeight; }
            set { SetProperty(ref _paperHeight, value); }
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

                PageLeftNumSet();
            }
        }

        public int RightNum
        {
            get { return _rightNum; }
            set
            {
                SetProperty(ref _rightNum, value);

                PageRightNumSet();
            }
        }

        public int TotalNum
        {
            get { return _totalNum; }
            set { SetProperty(ref _totalNum, value); }
        }

        public BookState BookState
        {
            get { return _bookState; }
            set { SetProperty(ref _bookState, value); }
        }

        public bool IsRightExist
        {
            get { return _isRightExist; }
            set { SetProperty(ref _isRightExist, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand PageBackCommand =>
            _pageBackCommand ?? (_pageBackCommand = new DelegateCommand(PageBack, CanPageBack).ObservesProperty(() => LeftNum));

        public DelegateCommand PageForwardCommand =>
            _pageForwardCommand ?? (_pageForwardCommand = new DelegateCommand(PageForward, CanPageForward).ObservesProperty(() => RightNum));

        #endregion Command

        #region Constructor

        public BookViewVM()
        {
        }

        #endregion Constructor

        #region Method

        private void EditBookChanged()
        {
            EditBook.BookPages.CollectionChanged += BookPages_CollectionChanged;

            PaperWidth = EditBook.PaperSize.GetWidthPixel();
            PaperHeight = EditBook.PaperSize.GetHeightPiexl();

            TotalNum = EditBook.BookPages.Count;
            LeftNum = -1;
            RightNum = 0;

            PageBackCommand.RaiseCanExecuteChanged();
            PageForwardCommand.RaiseCanExecuteChanged();
        }

        private void BookPages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TotalNum = EditBook.BookPages.Count;

            if (e.Action == NotifyCollectionChangedAction.Remove
                && TotalNum % 2 == 0)
            {
                PageBack();
            }
            else
            {
                LeftNum = LeftNum;
                RightNum = RightNum;
            }

            PageBackCommand.RaiseCanExecuteChanged();
            PageForwardCommand.RaiseCanExecuteChanged();
        }

        private bool CanPageBack()
        {
            return LeftNum > -1;
        }

        private void PageBack()
        {
            LeftNum -= 2;
            RightNum -= 2;
        }

        private bool CanPageForward()
        {
            return RightNum < TotalNum + 2;
        }

        private void PageForward()
        {
            LeftNum += 2;
            RightNum += 2;
        }

        private void PageLeftNumSet()
        {
            if (LeftNum < 1)
            {
                LeftPage = null;
                BookState = BookState.FrontCover;
            }
            else if (LeftNum > TotalNum)
            {
                LeftPage = EditBook.BackCover;
                BookState = BookState.BackCover;
            }
            else
            {
                LeftPage = EditBook.BookPages[LeftNum - 1];
                BookState = BookState.Page;
            }

        }

        private void PageRightNumSet()
        {
            IsRightExist = true;

            if (RightNum < 2)
            {
                RightPage = EditBook.FrontCover;
            }
            else if (RightNum > TotalNum)
            {
                RightPage = null;
                if (TotalNum % 2 == 1)
                {
                    IsRightExist = false;
                }
            }
            else
            {
                RightPage = EditBook.BookPages[RightNum - 1];
            }
        }

        #endregion Method
    }
}
