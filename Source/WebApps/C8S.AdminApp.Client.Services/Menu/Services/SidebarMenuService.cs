using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Notifications;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.Domain;
using C8S.Domain.Features;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Services;

public sealed class SidebarMenuService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService) : ISidebarMenuService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuService> _logger = loggerFactory.CreateLogger<SidebarMenuService>();
    #endregion

    #region Constants & ReadOnlys
    private static readonly IEnumerable<MenuGroup> MenuGroups = 
    [
        new() { Entity = DomainEntity.Request, Display = "Requests", IconString = C8SConstants.Icons.Request, Url = "requests" },
        new() { Entity = DomainEntity.Contact, Display = "Contacts", IconString = C8SConstants.Icons.Contact, Url = "contacts" },
        new() { Entity = DomainEntity.Organization, Display = "Organizations", IconString = C8SConstants.Icons.Organization, Url = "organizations" },
        new() { Entity = DomainEntity.Site, Display = "Sites", IconString = C8SConstants.Icons.Site, Url = "sites" },
        new() { Entity = DomainEntity.Order, Display = "Orders", IconString = C8SConstants.Icons.Order, Url = "orders" },
        new() { Entity = DomainEntity.Sku, Display = "Skus", IconString = C8SConstants.Icons.Sku, Url = "skus" }
    ];

    private static readonly Dictionary<DomainEntity, Dictionary<int, MenuItem>> MenuItemsLookup = new();
    #endregion

    #region Public Methods
    public void Initialize(IServiceProvider provider)
    {
        pubSubService.Subscribe<NavigationChange>(HandleNavigationChangeNotification);
    }

    public void Dispose()
    {
        pubSubService.Unsubscribe<NavigationChange>(HandleNavigationChangeNotification);
    }
    #endregion

    #region Query Handlers
    public Task<DomainResponse<IEnumerable<MenuGroup>>> Handle(MenuGroupsQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(DomainResponse<IEnumerable<MenuGroup>>.CreateSuccessResponse(MenuGroups));

    public Task<DomainResponse<IEnumerable<MenuItem>>> Handle(MenuItemsQuery query, CancellationToken cancellationToken = default)
    {
        if (!MenuItemsLookup.TryGetValue(query.Entity, out var menuItems))
            menuItems = [];

        var response =
            DomainResponse<IEnumerable<MenuItem>>.CreateSuccessResponse(menuItems.Values);

        return Task.FromResult(response);
    }
    #endregion

    #region Event Handlers

    public async Task HandleNavigationChangeNotification(NavigationChange navigationChange)
    {
        _logger.LogDebug("NavigationChange={@Change}", navigationChange);

        if (navigationChange is not { Action: NavigationAction.Open, IdValue: not null }) return;

        var idValue = navigationChange.IdValue.Value;

        // create the dictionary for the entity, if it doesn't yet exist
        if (!MenuItemsLookup.TryGetValue(navigationChange.Entity, out var menuItems))
        {
            menuItems = new Dictionary<int, MenuItem>();
            MenuItemsLookup.Add(navigationChange.Entity, menuItems);
        }

        // if we've already got the key, we're done
        if (menuItems.ContainsKey(idValue)) return;

        // otherwise, create and publish the change
        var menuItem = navigationChange.ToMenuItem(
            createDisplay:() => $"DISPLAY {navigationChange.IdValue.ToString()}", 
            createUrl:() => $"request/{idValue}");
        menuItems.Add(idValue, menuItem);

        await pubSubService.Publish(new MenuChange() { Entity = navigationChange.Entity })
            .ConfigureAwait(false);
    }
    #endregion

}