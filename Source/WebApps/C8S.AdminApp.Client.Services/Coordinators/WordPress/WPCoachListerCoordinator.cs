using System.Diagnostics;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Notifications;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.WordPress;

public sealed class WPCoachListerCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    //private readonly ILogger<WPCoachListerCoordinator> _logger = loggerFactory.CreateLogger<WPCoachListerCoordinator>();

    #region Component Properties
    public RadzenDataGrid<WPUserDetails> DataGrid { get; set; } = null!;
    #endregion
    
    #region Component Methods
    public EventCallback<WPUserDetails>? CoachSelected { get; set; }
    #endregion

    #region Public Properties
    public IList<WPUserDetails> Coaches { get; set; } = new List<WPUserDetails>();
    public IList<WPUserDetails> SelectedCoaches { get; set; } = [];
    public int TotalCount { get; set; } 

    public bool IsLoading { get; set; } = false;
    #endregion

    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();

        PubSubService.Subscribe<WPUsersUpdated>(HandleWPUsersUpdated);
        Task.Run(async () => await DataGrid.RefreshDataAsync());
    }

    public override void TearDown()
    {
        base.TearDown();

        PubSubService.Unsubscribe<WPUsersUpdated>(HandleWPUsersUpdated);
    }
    #endregion

    #region Public Methods
    public void LoadCoaches(LoadDataArgs args) =>
    Task.Run(async () => await LoadCoachesAsync(args));

    public async Task HandleRowSelected(WPUserDetails? arg) => 
        await CoachSelected!.Value.InvokeAsync(arg);
    #endregion

    #region Private Methods
    private async Task LoadCoachesAsync(LoadDataArgs _)
    {
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var response = await GetQueryResults<WPUsersListQuery, WrappedListResponse<WPUserDetails>>(
                new WPUsersListQuery() { IncludeRoles = ["Coach"] }) ??
                                    throw new UnreachableException("GetQueryResults returned null");
            if (response is { Success: false } or { Result: null })
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            Coaches = response.Result;
            TotalCount = response.Total;

            IsLoading = false;
            await PerformComponentRefresh();

            await HandleRowSelected(null);
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }

    private async Task HandleWPUsersUpdated(WPUsersUpdated _) =>
        await DataGrid.RefreshDataAsync(); 
    #endregion
}