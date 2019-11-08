using Memories.Business.Enums;
using Memories.Core;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;

namespace Memories.Modules.NewBook.ViewModels
{
    public class InputBookInfoViewVM : NavigationViewModelBase
    {
        public const int VIEW_INDEX = 0;

        #region Field

        private string _bookTitle;
        private string _writer;
        private string _filePath;
        private PaperSize _selectedPaperSize;

        private DelegateCommand _selectFilePathCommand;
        private NewBookNavigationParameter _naviParam;

        private readonly IFileService _fileService;

        #endregion Field

        #region Property

        public string BookTitle
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

        #region Constructor

        public InputBookInfoViewVM(IFileService fileService)
        {
            PropertyChanged += InputBookInfo_PropertyChanged;
            _fileService = fileService;
        }

        #endregion Constructor

        #region Method

        private void SelectFilePath()
        {
            FilePath = _fileService.SaveFilePath() ?? FilePath;
        }

        private void InputBookInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPaperSize))
            {
                _naviParam.IsCompleted[BookLayoutSelectViewVM.VIEW_INDEX] = false;
            }

            if (AllCompleted())
            {
                _naviParam.IsCompleted[VIEW_INDEX] = true;

                _naviParam.InputBook.Title = BookTitle;
                _naviParam.InputBook.Writer = Writer;
                _naviParam.InputBook.PaperSize = SelectedPaperSize;
                _naviParam.BookPath = FilePath;
            }
            else
            {
                _naviParam.IsCompleted[VIEW_INDEX] = false;
            }
        }

        private bool AllCompleted()
        {
            return !(string.IsNullOrWhiteSpace(BookTitle)
                || string.IsNullOrWhiteSpace(Writer)
                || string.IsNullOrWhiteSpace(FilePath));
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add(nameof(PaperSize), SelectedPaperSize);
            navigationContext.Parameters.Add("Parameter", _naviParam);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _naviParam = navigationContext.Parameters["Parameter"] as NewBookNavigationParameter;
            _naviParam.NowPage = VIEW_INDEX;
        }

        #endregion Method
    }
}
