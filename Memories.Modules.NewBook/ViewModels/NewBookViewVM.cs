using Memories.Core;
using Prism.Commands;
using System.Windows;

namespace Memories.Modules.NewBook.ViewModels
{
    public class NewBookViewVM : DialogViewModelBase
    {
        #region Field

        private DelegateCommand _previousCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _checkCommand;

        #endregion Field

        #region Property



        #endregion Property

        #region Command
        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(Previous, CanPrevious));

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(Next, CanNext));

        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(Cancel));

        public DelegateCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new DelegateCommand(Check, CanCheck));

        #endregion Command

        #region Constructor

        #endregion Constructor

        #region Method

        // Temporarily method for debug
        void Previous()
        {

        }

        bool CanPrevious()
        {
            return true;
        }

        void Next()
        {

        }

        bool CanNext()
        {
            return true;
        }

        void Cancel()
        {
            
        }

        void Check()
        {
            // Make Book with Information
        }

        bool CanCheck()
        {
            return false;
        }

        #endregion Method
    }
}
