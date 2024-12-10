using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum InvoiceStatus
{
    [Label("Pending")]
    Pending,
    [Label("Paid")]
    Paid,
    [Label("Canceled")]
    Canceled,
    [Label("Archived")]
    Archived
}