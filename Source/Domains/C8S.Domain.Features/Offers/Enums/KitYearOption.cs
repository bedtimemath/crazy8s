using SC.Common.Attributes;

namespace C8S.Domain.Features.Offers.Enums;

[Flags]
public enum KitYearOption
{
    [Label("Before F21")]
    PreF21 = 0x1,
    [Label("F21 to F23")]
    F21ToF23 = 0x2,
    [Label("F24 and After")]
    F24Plus = 0x4,
}