using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum OrderStatus
{
    [Label("Ordered")]
    Ordered,
    [Label("Processing")]
    Processing,
    [Label("Shipped")]
    Shipped,
    [Label("Canceled")]
    Canceled,
    [Label("Returned")]
    Returned
}