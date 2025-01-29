using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public record OpenNavigation: ICQRSCommand
{
    public NavigationGroup Group { get; init; }
    public int? IdValue { get; init; }
}