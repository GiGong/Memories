using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Extensions;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Windows;

namespace Memories.Modules.EditBook.ViewModels
{
    public class EditBookViewVM : DialogViewModelBase
    {
        #region Field

        private DelegateCommand _openCommand;

        private DelegateCommand _openStartWindowCommand;

        #endregion Field

        #region Property

        private Book _editBook;
        public Book EditBook
        {
            get { return _editBook; }
            set { SetProperty(ref _editBook, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand OpenCommand =>
            _openCommand ?? (_openCommand = new DelegateCommand(Open));

        public DelegateCommand OpenStartWindowCommand =>
            _openStartWindowCommand ?? (_openStartWindowCommand = new DelegateCommand(OpenStartWindow));

        #endregion Command

        #region Constructor

        public EditBookViewVM()
        {
            Title = (string)Application.Current.Resources["Program_Name"];
        }

        #endregion Constructor

        #region Method

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);

            if (parameters.ContainsKey("NewBook"))
            {
                EditBook = parameters.GetValue<Book>("NewBook");
            }
            else if (parameters.ContainsKey("LoadBook"))
            {
                EditBook = parameters.GetValue<Book>("LoadBook");
            }
        }

        void OpenStartWindow()
        {
            RaiseRequestClose(new DialogResult(ButtonResult.Retry));
        }

        void Open()
        {
            
        }

        #endregion Method
    }
}
