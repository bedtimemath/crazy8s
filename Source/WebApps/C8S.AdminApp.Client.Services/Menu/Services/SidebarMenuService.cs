using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.Domain;
using C8S.Domain.Features;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Menu.Services;

public sealed class SidebarMenuService(
    ILoggerFactory loggerFactory) : ISidebarMenuService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuService> _logger = loggerFactory.CreateLogger<SidebarMenuService>();
    #endregion

    #region Constants & ReadOnlys
    private static readonly IEnumerable<MenuGroup> NavigationGroups = 
    [
        new() { Entity = DomainEntity.Request, Display = "Requests", IconString = C8SConstants.Icons.Request, Url = "requests" },
        new() { Entity = DomainEntity.Contact, Display = "Contacts", IconString = C8SConstants.Icons.Contact, Url = "contacts" },
        new() { Entity = DomainEntity.Organization, Display = "Organizations", IconString = C8SConstants.Icons.Organization, Url = "organizations" },
        new() { Entity = DomainEntity.Site, Display = "Sites", IconString = C8SConstants.Icons.Site, Url = "sites" },
        new() { Entity = DomainEntity.Order, Display = "Orders", IconString = C8SConstants.Icons.Order, Url = "orders" },
        new() { Entity = DomainEntity.Sku, Display = "Skus", IconString = C8SConstants.Icons.Sku, Url = "skus" }
    ];
    #endregion

    #region Query Handlers
    public Task<DomainResponse<IEnumerable<MenuGroup>>> Handle(MenuGroupsQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(DomainResponse<IEnumerable<MenuGroup>>.CreateSuccessResponse(NavigationGroups));

    public Task<DomainResponse<IEnumerable<MenuItem>>> Handle(MenuItemsQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Query: {@Query}", query);

        IEnumerable<MenuItem> returnList = query.Entity switch
        {
            DomainEntity.Request =>
            [
                new MenuItem()
                    { Display = "Blah D. Blah", Entity = DomainEntity.Request, Url = "request/36232", IdValue = 36232 }
            ],
            _ => []
        };

        return Task.FromResult(DomainResponse<IEnumerable<MenuItem>>.CreateSuccessResponse(returnList));
    }
    #endregion

}