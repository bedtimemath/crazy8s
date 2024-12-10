using SC.Common.Attributes;

namespace C8S.Domain.Enums;

public enum ClubStatus
{
    [Label("Potential")]
    Potential,
    [Label("Active")]
    Active,
    [Label("Running")]
    Running,
    [Label("Complete")]
    Complete,
    [Label("Canceled")]
    Canceled,
    [Label("Archived")]
    Archived
}