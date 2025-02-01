using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.Domain.Features;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Commands;

public record NavigationCommand : ICQRSCommand
{
    public NavigationAction Action { get; init; }
    public DomainEntity Entity { get; init; }
    public string PageUrl { get; init; } = null!;
    public int? IdValue { get; init; }
    public string? JsonDetails { get; init; }
}