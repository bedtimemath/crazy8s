using C8S.AdminApp.Client.Services.Pages;
using SC.Common.Interfaces;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public interface INavigationService: IInitializable, IDisposable,
    ICQRSCommandHandler<OpenPageCommand>,
    ICQRSCommandHandler<ClosePageCommand>
{
}