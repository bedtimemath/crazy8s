using System.Diagnostics;
using C8S.WordPress.Abstractions.Commands;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Abstractions.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.WordPress;

public sealed class WPCoachEditorCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    //private readonly ILogger<WPCoachEditorCoordinator> _logger = loggerFactory.CreateLogger<WPCoachEditorCoordinator>();

    #region Public Properties
    public WPUserDetails Coach { get; private set; } = null!;
    public IList<WPRoleDetails> Roles { get; set; } = [];
    public IEnumerable<string> SelectedSlugs { get; set; } = [];

    public bool IsLoading { get; set; } = false;
    public bool HasChanged { get; set; } = false;
    #endregion

    #region SetUp / TearDown
    public override void SetUp()
    {
        base.SetUp();
        Task.Run(async () => await LoadRolesAsync());
    }
    #endregion

    #region Public Methods
    public void SetCoach(WPUserDetails coach)
    {
        Coach = coach;
        SelectedSlugs = coach.RoleSlugs;
        Task.Run(async () => await PerformComponentRefresh());
    }

    public async Task HandleSaveClicked()
    {
        var response = await GetCommandResults<WPUserUpdateRolesCommand, WrappedResponse<WPUserDetails>>(
            new WPUserUpdateRolesCommand()
            {
                UserName = Coach.UserName,
                Roles = SelectedSlugs.ToList()
            });
        if (response is { Success: false } or { Result: null })
            throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

        SetCoach(response.Result);
    }

    public async Task HandleDeleteClicked()
    {
        var response = await GetCommandResults<WPUserDeleteCommand, WrappedResponse>(
            new WPUserDeleteCommand() { UserName = Coach.UserName });
        if (response is { Success: false })
            throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");
    }

    public void HandleRolesChanged(IEnumerable<string> values)
    {
        var valuesList = values.ToList();
        HasChanged = Coach.RoleSlugs.Except(valuesList).Any() || 
                     valuesList.Except(Coach.RoleSlugs).Any();
        SelectedSlugs = valuesList;
    }
    #endregion

    #region Private Methods
    private async Task LoadRolesAsync()
    {
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

            var response = await GetQueryResults<WPRolesListQuery, WrappedListResponse<WPRoleDetails>>(new WPRolesListQuery()) ??
                           throw new UnreachableException("GetQueryResults returned null");
            if (response is { Success: false } or { Result: null })
                throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");

            Roles = response.Result;

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }
    #endregion
}