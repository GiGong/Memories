using Memories.Views;
using Prism.Commands;

namespace Memories.ViewModels
{
    public class MainWindowVM
    {
        #region Field

        private DelegateCommand _newBookCommand;
        private DelegateCommand _loadBookCommand;

        #endregion

        #region Command

        public DelegateCommand NewBookCommand =>
            _newBookCommand ?? (_newBookCommand = new DelegateCommand(NewBook));

        public DelegateCommand LoadBookCommand =>
            _loadBookCommand ?? (_loadBookCommand = new DelegateCommand(LoadBook));

        #endregion Command

        #region Method

        private void NewBook()
        {
            new NewBookWindow().Show();
        }

        void LoadBook()
        {

        }

        #endregion Method

    }
}
