using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum TicketStatus
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