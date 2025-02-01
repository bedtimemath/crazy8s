using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public interface ISidebarMenuService :
    ICQRSQueryHandler<NavigationGroupsQuery, DomainResponse<IEnumerable<NavigationGroup>>>
{
}