using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum SaleStatus
{
    [Label("Potential")]
    Potential,
    [Label("Pending")]
    Pending,
    [Label("Invoiced")]
    Invoiced,
    [Label("Paid")]
    Paid,
    [Label("Complete")]
    Complete,
    [Label("Canceled")]
    Canceled,
    [Label("Archived")]
    Archived
}