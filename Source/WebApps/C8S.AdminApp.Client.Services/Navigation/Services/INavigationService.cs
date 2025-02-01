using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public interface INavigationService :
    ICQRSCommandHandler<NavigationCommand>,
    ICQRSQueryHandler<NavigationGroupsQuery, DomainResponse<IEnumerable<NavigationGroup>>>
{
}