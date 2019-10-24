using Memories.Modules.EditBook.Enums;
using Prism.Mvvm;

namespace Memories.Modules.EditBook.Parameters
{
    public class ExportParameter : BindableBase
    {
        private string _title;
        private ExportType _type;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ExportType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public ExportParameter(string title, ExportType type)
        {
            Title = title;
            Type = type;
        }
    }
}
