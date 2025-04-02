using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SC.Common.Razor.Extensions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Tickets;

public class TicketsListCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    IJSRuntime jsRuntime) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region Constants & ReadOnlys
    public const string ListerContainerId = "ticket-list-container";
    #endregion

    #region ReadOnly Constructor Variables
    //private readonly ILogger<TicketsListCoordinator> _logger = loggerFactory.CreateLogger<TicketsListCoordinator>();
    #endregion

    #region Public Events
    public event EventHandler? FilterChanged;
    public void RaiseFilterChanged() => FilterChanged?.Invoke(this, EventArgs.Empty);

    public event EventHandler? ListUpdated;
    public void RaiseListUpdated() => ListUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Methods
    public async Task ScrollListToTop()
    {
        await jsRuntime.ScrollToTop(ListerContainerId);
    }
    #endregion

}