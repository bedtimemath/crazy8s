using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Interfaces;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Menu.Services;

public interface ISidebarMenuService : IServiceInitializable, IDisposable,
    ICQRSQueryHandler<MenuGroupsQuery, WrappedResponse<IEnumerable<MenuGroup>>>,
    ICQRSQueryHandler<MenuSinglesQuery, WrappedResponse<IEnumerable<MenuSingle>>>,
    ICQRSQueryHandler<MenuItemsQuery, WrappedResponse<IEnumerable<MenuItem>>>
{
}