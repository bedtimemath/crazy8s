using SC.Common.Attributes;

namespace C8S.WordPress.Abstractions.Models;

public enum WPPageStatus
{
    [Label("Draft")]
    Draft,
    [Label("Active")]
    Active,
    [Label("Inactive")]
    Inactive
}