using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using C8S.Domain;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public sealed class SidebarMenuService(
    ILoggerFactory loggerFactory) : ISidebarMenuService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuService> _logger = loggerFactory.CreateLogger<SidebarMenuService>();
    #endregion

    #region Query Handlers
    public Task<DomainResponse<IEnumerable<NavigationGroup>>> Handle(NavigationGroupsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<NavigationGroup> list = new List<NavigationGroup>()
        {
            new() { Entity = NavigationEntity.Requests, Display = "Requests", IconString = C8SConstants.Icons.Request },
            new() { Entity = NavigationEntity.Contacts, Display = "Contacts", IconString = C8SConstants.Icons.Contact },
            new() { Entity = NavigationEntity.Organizations, Display = "Organizations", IconString = C8SConstants.Icons.Organization },
            new() { Entity = NavigationEntity.Sites, Display = "Sites", IconString = C8SConstants.Icons.Site },
            new() { Entity = NavigationEntity.Orders, Display = "Orders", IconString = C8SConstants.Icons.Order },
            new() { Entity = NavigationEntity.Skus, Display = "Skus", IconString = C8SConstants.Icons.Sku }
        };
        return Task.FromResult(
            DomainResponse<IEnumerable<NavigationGroup>>.CreateSuccessResponse(list));
    }
    #endregion

}