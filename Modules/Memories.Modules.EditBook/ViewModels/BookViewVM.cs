using Memories.Business.Models;
using Prism.Mvvm;
using Prism.Regions;

namespace Memories.Modules.EditBook.ViewModels
{
    public class BookViewVM : BindableBase
    {
        #region Field

        private Book _editBook;
        private BookPage _leftPage;
        private BookPage _rightPage;

        private int _currentPage;
        private int _totalPage;

        #endregion Field

        #region Property

        public Book EditBook
        {
            get { return _editBook; }
            set 
            {
                SetProperty(ref _editBook, value);

                TotalPage = EditBook.BookPages.Count;
                CurrentPage = 1;
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

        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        public int TotalPage
        {
            get { return _totalPage; }
            set { SetProperty(ref _totalPage, value); }
        }

        #endregion Property

        public BookViewVM()
        {
        }
    }
}
