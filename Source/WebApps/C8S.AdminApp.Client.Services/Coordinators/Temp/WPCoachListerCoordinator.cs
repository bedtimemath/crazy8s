using System.Diagnostics;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Notifications;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Temp;

public sealed class WPCoachListerCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    //private readonly ILogger<WPCoachListerCoordinator> _logger = loggerFactory.CreateLogger<WPCoachListerCoordinator>();

    public RadzenDataGrid<WPUserDetails> DataGrid { get; set; } = null!;

    public IEnumerable<WPUserDetails> Coaches { get; set; } = new List<WPUserDetails>();
    public IList<WPUserDetails> SelectedCoaches { get; set; } = [];
    public int TotalCount { get; set; }

    public bool IsLoading { get; set; } = false;

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

    public void LoadCoaches(LoadDataArgs args) =>
        Task.Run(async () => await LoadCoachesAsync(args));

    private async Task LoadCoachesAsync(LoadDataArgs _)
    {
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var wpUsersListResult = await GetQueryResults<WPUsersListQuery, DomainResponse<WPUsersListResults>>(
                new WPUsersListQuery() { IncludeRoles = [ "Coach" ]}) ??
                                    throw new UnreachableException("GetQueryResults returned null");
            
            if (!wpUsersListResult.Success || wpUsersListResult.Result == null)
                throw wpUsersListResult.Exception?.ToException() ??
                      new Exception("Error occurred loading persons");

            Coaches = wpUsersListResult.Result.Items;
            TotalCount = wpUsersListResult.Result.Total;

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }

    private async Task HandleWPUsersUpdated(WPUsersUpdated _) =>
        await DataGrid.RefreshDataAsync();
}