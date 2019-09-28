using Memories.Business.Converters;
using System.ComponentModel;

namespace Memories.Business.Enums
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PaperSize
    {
        [Description("A3 : 420 * 297")]
        A3,
        [Description("A4 : 297 * 210")]
        A4,
        [Description("A5 : 210 * 148")]
        A5,
        [Description("A6 : 148 * 105")]
        A6,
        [Description("B4 : 374 * 254")]
        B4,
        [Description("B5 : 257 * 188")]
        B5,
        [Description("B6 : 188 * 128")]
        B6,
        [Description("비규격 : 사용자 설정")]
        비규격
    }

    public static class PaperSizeExtension
    {
        public static double GetWidth(this PaperSize paperSize)
        {
            switch (paperSize)
            {                
                case PaperSize.비규격:
                default:
                    return -1;

                case PaperSize.A3:
                    return 297;
                case PaperSize.A4:
                    return 210;
                case PaperSize.A5:
                    return 148;
                case PaperSize.A6:
                    return 105;
                case PaperSize.B4:
                    return 254;
                case PaperSize.B5:
                    return 188;
                case PaperSize.B6:
                    return 128;
            }
        }

        public static double GetHeight(this PaperSize paperSize)
        {
            switch (paperSize)
            {                
                case PaperSize.비규격:
                default:
                    return -1;

                case PaperSize.A3:
                    return 420;
                case PaperSize.A4:
                    return 297;
                case PaperSize.A5:
                    return 210;
                case PaperSize.A6:
                    return 148;
                case PaperSize.B4:
                    return 374;
                case PaperSize.B5:
                    return 257;
                case PaperSize.B6:
                    return 188;
            }
        }
    }
}
