using C8S.AdminApp.Client.Services.Navigation.Commands;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public interface INavigationService :
    ICQRSCommandHandler<NavigationCommand>
{
}