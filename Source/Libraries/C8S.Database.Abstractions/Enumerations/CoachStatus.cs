using SC.Common.Attributes;

namespace C8S.Database.Abstractions.Enumerations;

public enum CoachStatus
{
    [Label("Pending")]
    Pending,
    [Label("Active")]
    Active,
    [Label("Canceled")]
    Canceled
}