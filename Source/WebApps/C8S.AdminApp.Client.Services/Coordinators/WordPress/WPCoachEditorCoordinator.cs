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
    private readonly ILogger<WPCoachEditorCoordinator> _logger = loggerFactory.CreateLogger<WPCoachEditorCoordinator>();

    #region Public Properties
    public WPUserDetails Coach { get; private set; } = null!;
    public IList<WPRoleDetails> Roles { get; set; } = [];
    public IEnumerable<string> SelectedSlugs { get; set; } = [];

    public string? MagicLink { get; set; }

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
                Id = Coach.Id,
                Roles = SelectedSlugs.ToList()
            });
        if (response is { Success: false } or { Result: null })
            throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");
    }

    public async Task HandleDeleteClicked()
    {
        var response = await GetCommandResults<WPUserDeleteCommand, WrappedResponse>(
            new WPUserDeleteCommand() { Id = Coach.Id });
        if (response is { Success: false })
            throw response.Exception?.ToException() ?? new UnreachableException("Missing exception");
    }

    public async Task HandleCreateMagicLinkClicked()
    {
        var magicLinkResponse = await GetCommandResults<WPUserCreateMagicLinkCommand, WrappedResponse<string>>(
            new WPUserCreateMagicLinkCommand() { Id = Coach.Id });
        if (magicLinkResponse is { Success: false } or { Result: null })
            throw magicLinkResponse.Exception?.ToException() ?? new UnreachableException("Missing exception");

        MagicLink = magicLinkResponse.Result;
        await PerformComponentRefresh();
    }

    public async Task HandleClearMagicLinkClicked()
    {
        MagicLink = null;
        await PerformComponentRefresh();
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

    //private Task HandleDataChange(DataChange dataChange)
    //{
    //    if ((dataChange is not { Action: DataChangeAction.Modified, EntityName: C8SConstants.Entities.WPUser })
    //        || (dataChange.EntityId != Coach.Id)) return Task.CompletedTask;

    //    if (String.IsNullOrEmpty(dataChange.JsonDetails))
    //        throw new UnreachableException("DataChange missing JasonDetails");
    //    var coach = JsonSerializer.Deserialize<WPUserDetails>(dataChange.JsonDetails) ??
    //                throw new UnreachableException("JsonDetails could not be deserialized.");

    //    SetCoach(coach);

    //    return Task.CompletedTask;
    //}
    #endregion
}