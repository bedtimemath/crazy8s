using SC.Common.Attributes;

namespace C8S.Database.Abstractions.Enumerations;

public enum SkuStatus
{
    [Label("Draft")]
    Draft,
    [Label("Active")]
    Active,
    [Label("Inactive")]
    Inactive
}