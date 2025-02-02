using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using SC.Common.Interactions;
using SC.Common.Interfaces;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Services;

public interface ISidebarMenuService : IServiceInitializable, IDisposable,
    ICQRSQueryHandler<MenuGroupsQuery, DomainResponse<IEnumerable<MenuGroup>>>,
    ICQRSQueryHandler<MenuItemsQuery, DomainResponse<IEnumerable<MenuItem>>>
{
}