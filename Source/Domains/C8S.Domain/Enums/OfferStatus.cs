using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum OfferStatus
{
    [Label("Draft")]
    Draft,
    [Label("Active")]
    Active,
    [Label("Inactive")]
    Inactive
}