﻿using Memories.Modules.EditBook.Enums;

namespace Memories.Modules.EditBook.Parameters
{
    public class ExportParameter : ParameterBase
    {
        private ExportType _type;

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
