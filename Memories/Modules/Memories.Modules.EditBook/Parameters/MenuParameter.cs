using Memories.Modules.EditBook.Enums;

namespace Memories.Modules.EditBook.Parameters
{
    public class MenuParameter : ParameterBase
    {
        private MenuType _type;

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
