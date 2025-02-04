using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using C8S.Domain.Features;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Services;

public sealed class NavigationService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    NavigationManager navigationManager) : INavigationService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NavigationService> _logger = loggerFactory.CreateLogger<NavigationService>();
    #endregion

    #region Private Variables
    private readonly Stack<PageRecord> _history = new();
    private PageRecord? _currentPage = null;
    #endregion

    #region Public Methods
    public void Initialize(IServiceProvider _)
    {
        _currentPage = new PageRecord()
        {
            PageUrl = navigationManager.GetRelativeUrl()
        };
    }

    public void Dispose()
    {
    }
    #endregion

    #region Command Handlers
    public Task<DomainResponse<string>> Handle(CurrentUrlQuery query, CancellationToken cancellation) =>
        Task.FromResult(DomainResponse<string>.CreateSuccessResponse(navigationManager.GetRelativeUrl()));

    public async Task Handle(NavigationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Action}]: {Group} [{IdValue}]", command.Action, command.Entity, command.IdValue);

        // now do the work
        switch (command.Action)
        {
            case NavigationAction.Open:
                await HandleOpen(command, cancellationToken);
                break;
            case NavigationAction.Close:
                await HandleClose(command, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion

    #region Private Methods
    private async Task HandleOpen(NavigationCommand command, CancellationToken cancellationToken)
    {
        // we don't want to get stuck in an endless loop, so don't navigate to self
        var currentUrl = navigationManager.GetRelativeUrl();
        if (currentUrl == command.PageUrl) return;

        // add this to the stack
        _history.Push(new PageRecord() { PageUrl = command.PageUrl, Entity = command.Entity, IdValue = command.IdValue });

        // now perform the navigation and publish the change
        await Task.Run(() => navigationManager.NavigateTo(command.PageUrl), cancellationToken);
        pubSubService.Publish(new NavigationChange()
        {
            OldUrl = currentUrl,
            Action = command.Action,
            Entity = command.Entity,
            PageUrl = command.PageUrl,
            IdValue = command.IdValue
        });
    }
    private async Task<NavigationChange> HandleClose(NavigationCommand command, CancellationToken cancellationToken)
    {
        // get the most recent one from the stack
        if (!_history.TryPop(out var popped))
            popped = new PageRecord() { PageUrl = "home" };
        
        // close the existing by going to the old one
        await Task.Run(() => navigationManager.NavigateTo(popped.PageUrl), cancellationToken);

        
        var currentUrl = navigationManager.GetRelativeUrl();
        return new NavigationChange()
        {
            OldUrl = currentUrl,
            Action = command.Action,
            Entity = popped.Entity,
            PageUrl = popped.PageUrl,
            IdValue = popped.IdValue
        };
    }
    #endregion

    #region Private Records
    private record PageRecord
    {
        public string PageUrl { get; init; } = null!;
        public DomainEntity? Entity { get; init; }
        public int? IdValue { get; init; }
    }
    #endregion
}