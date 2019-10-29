using Memories.Business.Enums;

namespace Memories.Modules.EditBook.Parameters
{
    public class DrawParameter : ParameterBase
    {
        private BookUIEnum _type;

        public BookUIEnum Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public DrawParameter(string title, BookUIEnum type)
        {
            Title = title;
            Type = type;
        }
    }
}
