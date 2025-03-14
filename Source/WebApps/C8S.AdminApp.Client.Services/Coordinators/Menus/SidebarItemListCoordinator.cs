﻿using System.Diagnostics;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Menu.Notifications;
using C8S.AdminApp.Client.Services.Menu.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarItemListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarItemListCoordinator> _logger = loggerFactory.CreateLogger<SidebarItemListCoordinator>();
    #endregion
    
    #region Public Properties
    public MenuGroup Group { get; set; } = null!;
    public IEnumerable<MenuItem> MenuItems = [];
    #endregion

    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();
        
        PubSubService.Subscribe<MenuChange>(HandleMenuChange);
        Task.Run(async () => await GetMenuItems());
    }

    public override void TearDown()
    {
        base.TearDown();
        
        PubSubService.Unsubscribe<MenuChange>(HandleMenuChange);
    }
    #endregion

    #region Public Methods
    private async Task GetMenuItems()
    {
        if (Group.Entity == null)
            throw new UnreachableException("SidebarItemList shouldn't be invoked with an empty Entity");

        var response = await GetQueryResults<MenuItemsQuery, WrappedResponse<IEnumerable<MenuItem>>>(
            new MenuItemsQuery() { Entity = Group.Entity!.Value });

        MenuItems = response.Success ? response.Result! : [];
        await PerformComponentRefresh();
    }
    #endregion
    
    #region Event Handlers
    public async Task HandleMenuChange(MenuChange menuChange)
    {
        if (menuChange.Entity != Group.Entity) return;

        await GetMenuItems();
    }
    #endregion
}