﻿using Memories.Business.Enums;
using System.Collections.ObjectModel;

namespace Memories.Business.Models
{
    public class Book : BusinessBase
    {
        #region Field

        private string _title;
        private string _writer;

        private PaperSize _paperSize;

        private ObservableCollection<BookPage> _bookPages;

        private BookPage _frontCover;
        private BookPage _backCover;

        #endregion Field

        #region Property

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Writer
        {
            get { return _writer; }
            set { SetProperty(ref _writer, value); }
        }

        public PaperSize PaperSize
        {
            get { return _paperSize; }
            set
            {
                SetProperty(ref _paperSize, value);
            }
        }

        public ObservableCollection<BookPage> BookPages
        {
            get { return _bookPages; }
            set { SetProperty(ref _bookPages, value); }
        }

        public BookPage FrontCover
        {
            get { return _frontCover; }
            set { SetProperty(ref _frontCover, value); }
        }

        public BookPage BackCover
        {
            get { return _backCover; }
            set { SetProperty(ref _backCover, value); }
        }

        #endregion Property

        #region Constructor

        public Book()
        {
            Title = string.Empty;
            Writer = string.Empty;
            PaperSize = PaperSize.비규격;
            BookPages = new ObservableCollection<BookPage>();
            FrontCover = new BookPage();
            BackCover = new BookPage();
        }

        #endregion Constructor

        #region Method

        public Book Clone()
        {
            return new Book()
            {
                Title = Title,
                Writer = Writer,
                PaperSize = PaperSize,
                BookPages = new ObservableCollection<BookPage>(BookPages),
                FrontCover = FrontCover.Clone(),
                BackCover = BackCover.Clone()
            };
        }

        #endregion Method
    }
}
