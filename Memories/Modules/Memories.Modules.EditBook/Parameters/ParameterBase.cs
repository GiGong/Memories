using Prism.Mvvm;

namespace Memories.Modules.EditBook.Parameters
{
    public class ParameterBase : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
