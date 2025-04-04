﻿using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Razor.Extensions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Base;

public abstract class BaseListCoordinator<TListItem>(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IJSRuntime jsRuntime) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
    where TListItem: class, new()
{

    #region Public Events
    public event EventHandler? FilterChanged;
    public void RaiseFilterChanged() => FilterChanged?.Invoke(this, EventArgs.Empty);

    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Abstract Properties
    public abstract string ListContainerId { get; }
    #endregion

    #region Abstract Methods
    public abstract ValueTask<ItemsProviderResult<TListItem>>
        GetRows(ItemsProviderRequest request);
    #endregion

    #region Public Methods
    public async Task ScrollListToTop()
    {
        await jsRuntime.ScrollToTop(ListContainerId);
    }
    #endregion
}