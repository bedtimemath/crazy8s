using System.Diagnostics;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using Radzen;
using Radzen.Blazor;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Temp;

public sealed class WordPressCoachListerCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    private readonly ILogger<WordPressCoachListerCoordinator> _logger =
        loggerFactory.CreateLogger<WordPressCoachListerCoordinator>();

    public RadzenDataGrid<WPUserDetails> DataGrid { get; set; } = null!;

    public IEnumerable<WPUserDetails> Coaches { get; set; } = new List<WPUserDetails>();
    public IList<WPUserDetails> SelectedCoaches { get; set; } = [];
    public int TotalCount { get; set; }

    public bool IsLoading { get; set; } = false;

    public override void SetUp()
    {
        base.SetUp();
        LoadCoaches(new LoadDataArgs());
    }

    public void LoadCoaches(LoadDataArgs args) =>
        Task.Run(async () => await LoadCoachesAsync(args));

    private async Task LoadCoachesAsync(LoadDataArgs args)
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

            _logger.LogDebug("Skip={Skip}, Top={Top}, Total={Total}",
                args.Skip, args.Top, TotalCount); 

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }
}