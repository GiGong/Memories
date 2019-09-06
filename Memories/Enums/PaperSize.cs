using Memories.Converters;
using System.ComponentModel;

namespace Memories.Enums
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
}
