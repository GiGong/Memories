using Memories.Enums;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace Memories.ViewModels
{
    public class InputBookInfoViewVM : BindableBase
    {
        #region Field

        private string _bookTitle;
        private string _writer;
        private string _filePath;
        private PaperSize _selectedPaperSize;

        private DelegateCommand _selectFilePathCommand;

        #endregion Field

        #region Property

        public string Booktitle
        {
            get { return _bookTitle; }
            set { SetProperty(ref _bookTitle, value); }
        }

        public string Writer
        {
            get { return _writer; }
            set { SetProperty(ref _writer, value); }
        }

        public PaperSize SelectedPaperSize
        {
            get { return _selectedPaperSize; }
            set { SetProperty(ref _selectedPaperSize, value); }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        #endregion Property

        #region Command

        public DelegateCommand SelectFilePathCommand =>
            _selectFilePathCommand ?? (_selectFilePathCommand = new DelegateCommand(SelectFilePath));

        #endregion Command

        #region Method

        private void SelectFilePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
            }
        } 

        #endregion Method
    }
}
