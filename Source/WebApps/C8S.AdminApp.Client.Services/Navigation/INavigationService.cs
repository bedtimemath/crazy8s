using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public interface INavigationService: 
    ICQRSCommandHandler<NavigationCommand>
{ 
}