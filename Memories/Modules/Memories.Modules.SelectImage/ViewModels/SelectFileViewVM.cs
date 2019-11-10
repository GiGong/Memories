using Memories.Business;
using Memories.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace Memories.Modules.SelectImage.ViewModels
{
    public class SelectFileViewVM : BindableBase
    {
        #region Field

        private ImageParameter _selectedImage;

        private DelegateCommand _selectFileCommand;
        private readonly IFileService _fileService;

        #endregion Field

        public ImageParameter SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        public DelegateCommand SelectFileCommand =>
            _selectFileCommand ?? (_selectFileCommand = new DelegateCommand(ExecuteSelectFileCommand));

        public SelectFileViewVM(IFileService fileService)
        {
            _fileService = fileService;
        }

        private void ExecuteSelectFileCommand()
        {
            string filter = ExtentionFilters.ImageFiles;
            string path = _fileService.OpenFilePath(filter);

            if (path != null)
            {
                SelectedImage.SetSourceFromPath(path);
            }
        }
    }
}
