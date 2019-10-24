using Memories.Modules.EditBook.Enums;
using Prism.Mvvm;

namespace Memories.Modules.EditBook.Parameters
{
    public class MenuParameter : BindableBase
    {
        private string _title;
        private MenuType _type;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MenuType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public MenuParameter(string title, MenuType type)
        {
            Title = title;
            Type = type;
        }
    }
}
