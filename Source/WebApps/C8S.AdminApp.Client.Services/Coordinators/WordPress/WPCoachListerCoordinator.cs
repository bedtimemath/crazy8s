using System.Diagnostics;
using C8S.Domain;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Helpers.Base;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.PubSub.Services;
using SC.Common.PubSub;
using SC.Common.Responses;

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

        PubSubService.Subscribe<DataChange>(HandleDataChange);
        Task.Run(async () => await DataGrid.RefreshDataAsync());
    }

    public override void TearDown()
    {
        base.TearDown();

        PubSubService.Unsubscribe<DataChange>(HandleDataChange);
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

            var currentSelectionId = SelectedCoaches.FirstOrDefault()?.Id;

            var response = await GetQueryResults<WPUsersListQuery, WrappedListResponse<WPUserDetails>>(
                new WPUsersListQuery() { IncludeRoles = ["Coach"] }) ??
                                    throw new UnreachableException("GetQueryResults returned null");
            if (response is { Success: false } or { Result: null })
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            Coaches = response.Result;
            TotalCount = response.Total;

            IsLoading = false;
            await PerformComponentRefresh();

            // reselect if possible (must come after refresh)
            var selectRow = Coaches.All(c => c.Id != currentSelectionId) ? null :
                DataGrid.View.FirstOrDefault(r => r.Id == currentSelectionId);
            if (selectRow != null)
                await DataGrid.SelectRow(selectRow);
            else
                await HandleRowSelected(null);
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }

    private async Task HandleDataChange(DataChange dataChange)
    {
        if (dataChange is not { EntityName: C8SConstants.Entities.WPUser }) return;

        await DataGrid.RefreshDataAsync();
    }
    #endregion
}