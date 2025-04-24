using SC.Common.Razor.Navigation.Enums;

namespace SC.Common.Razor.Navigation.Models;

public record NavigationChange
{
    public NavigationAction Action { get; init; }
    public string PageUrl { get; init; } = null!;
}