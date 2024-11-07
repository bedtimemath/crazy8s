using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum CoachStatus
{
    [Label("Pending")]
    Pending,
    [Label("Active")]
    Active,
    [Label("Canceled")]
    Canceled
}