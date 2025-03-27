using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum KitStatus
{
    [Label("Draft")]
    Draft,
    [Label("Active")]
    Active,
    [Label("Inactive")]
    Inactive
}