using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Razor.Navigation.Enums;

namespace SC.Common.Razor.Navigation.Commands;

public record NavigationCommand : ICQRSCommand
{
    public NavigationAction Action { get; init; }
    public string PageUrl { get; init; } = null!;
}