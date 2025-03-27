using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum KitPageStatus
{
    [Label("Draft")]
    Draft,
    [Label("Active")]
    Active,
    [Label("Inactive")]
    Inactive
}