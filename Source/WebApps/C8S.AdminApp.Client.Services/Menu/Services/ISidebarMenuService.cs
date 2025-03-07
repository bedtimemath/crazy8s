﻿using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using SC.Common.Interfaces;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Services;

public interface ISidebarMenuService : IServiceInitializable, IDisposable,
    ICQRSQueryHandler<MenuGroupsQuery, WrappedResponse<IEnumerable<MenuGroup>>>,
    ICQRSQueryHandler<MenuSinglesQuery, WrappedResponse<IEnumerable<MenuSingle>>>,
    ICQRSQueryHandler<MenuItemsQuery, WrappedResponse<IEnumerable<MenuItem>>>
{
}