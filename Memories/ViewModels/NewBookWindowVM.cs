using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Memories.ViewModels
{
    public class NewBookWindowVM : BindableBase
    {
        #region Field

        private int _infoPage;


        private DelegateCommand _previousCommand;
        private DelegateCommand _nextCommand;
        private DelegateCommand<Window> _cancelCommand;
        private DelegateCommand _checkCommand;

        #endregion Field

        #region Property

        public int InfoPage
        {
            get { return _infoPage; }
            set
            {
                SetProperty(ref _infoPage, value);
                PreviousCommand.RaiseCanExecuteChanged();
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Property

        #region Command
        public DelegateCommand PreviousCommand =>
            _previousCommand ?? (_previousCommand = new DelegateCommand(Previous, CanPrevious));

        public DelegateCommand NextCommand =>
            _nextCommand ?? (_nextCommand = new DelegateCommand(Next, CanNext));

        public DelegateCommand<Window> CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand<Window>(Cancel));

        public DelegateCommand CheckCommand =>
            _checkCommand ?? (_checkCommand = new DelegateCommand(Check, CanCheck));

        #endregion Command

        #region Constructor

        public NewBookWindowVM()
        {


            InfoPage = 1;
        }

        #endregion Constructor

        #region Method

        // Temporarily method for debug
        void Previous()
        {
            InfoPage >>= 1;
        }

        bool CanPrevious()
        {
            return InfoPage > 1;
        }

        void Next()
        {
            InfoPage <<= 1;
        }

        bool CanNext()
        {
            return InfoPage < 2;
        }

        void Cancel(Window window)
        {
            window.Close();
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
