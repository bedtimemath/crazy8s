using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using SC.Common.Interactions;
using SC.Common.Interfaces;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public interface INavigationService :  IServiceInitializable, IDisposable,
    ICQRSCommandHandler<NavigationCommand>,
    ICQRSQueryHandler<CurrentUrlQuery, DomainResponse<string>>
{
}