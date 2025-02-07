﻿using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Notifications;
using C8S.AdminApp.Client.Services.Menu.Queries;
using C8S.Domain;
using C8S.Domain.Features;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Navigation.Enums;
using SC.Common.Client.Navigation.Models;
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

    private static readonly IEnumerable<MenuSingle> PreSingles =
    [
        new() { Display = "Home", IconString = C8SConstants.Icons.Home, Url = "home" },
    ];
    private static readonly IEnumerable<MenuGroup> MenuGroups = 
    [
        new() { Entity = DomainEntity.Request, Display = "Requests", IconString = C8SConstants.Icons.Request, Url = "requests" },
        new() { Entity = DomainEntity.Contact, Display = "Contacts", IconString = C8SConstants.Icons.Contact, Url = "persons" },
        new() { Entity = DomainEntity.Organization, Display = "Organizations", IconString = C8SConstants.Icons.Organization, Url = "organizations" },
        new() { Entity = DomainEntity.Site, Display = "Sites", IconString = C8SConstants.Icons.Site, Url = "sites" },
        new() { Entity = DomainEntity.Order, Display = "Orders", IconString = C8SConstants.Icons.Order, Url = "orders" },
        new() { Entity = DomainEntity.Sku, Display = "Skus", IconString = C8SConstants.Icons.Sku, Url = "skus" }
    ];
    private static readonly IEnumerable<MenuSingle> PostSingles =
    [
    ];

    private static readonly Dictionary<DomainEntity, Dictionary<int, MenuItem>> MenuItemsLookup = [];
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
    public Task<DomainResponse<IEnumerable<MenuSingle>>> Handle(MenuSinglesQuery query, CancellationToken cancellationToken = default) =>
        query.ShowBeforeOthers ?
        Task.FromResult(DomainResponse<IEnumerable<MenuSingle>>.CreateSuccessResponse(PreSingles)) :
        Task.FromResult(DomainResponse<IEnumerable<MenuSingle>>.CreateSuccessResponse(PostSingles));

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

    #region Command Handlers
    #endregion

    #region Event Handlers
    public Task HandleNavigationChangeNotification(NavigationChange navigationChange)
    {
        if (navigationChange is not { Action: NavigationAction.Open }) 
            return Task.CompletedTask;

        var entity = DomainUrlEx.GetDomainEntityFromUrl(navigationChange.PageUrl);
        if (entity == null) return Task.CompletedTask;
        var idValue = DomainUrlEx.GetIdValueFromUrl(navigationChange.PageUrl);
        if (idValue == null) return Task.CompletedTask;
        
        if (navigationChange.Action == NavigationAction.Open) HandleOpenChange(entity.Value, idValue.Value);

        return Task.CompletedTask;
    }
    #endregion

    #region Private Methods

    private void HandleOpenChange(DomainEntity entity, int idValue)
    {
        // create the dictionary for the entity, if it doesn't yet exist
        if (!MenuItemsLookup.TryGetValue(entity, out var menuItems))
        {
            menuItems = [];
            MenuItemsLookup.Add(entity, menuItems);
        }

        // if we've already got the key, we're done
        if (menuItems.ContainsKey(idValue)) return;

        // otherwise, create and publish the change
        var menuItem = new MenuItem() { Entity = entity, IdValue = idValue };
        menuItems.Add(idValue, menuItem);

        pubSubService.Publish(new MenuChange() { Entity = entity });
    }
    #endregion

}