using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.PubSub.Services;
using SC.Common.Razor.Extensions;
using SC.Common.Razor.Navigation.Commands;
using SC.Common.Razor.Navigation.Enums;
using SC.Common.Razor.Navigation.Models;
using SC.Common.Razor.Navigation.Queries;
using SC.Common.Responses;

namespace SC.Common.Razor.Navigation.Services;

public sealed class NavigationService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    NavigationManager navigationManager) : INavigationService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NavigationService> _logger = loggerFactory.CreateLogger<NavigationService>();
    #endregion

    #region Private Variables
    private readonly Stack<string> _history = new();
    private string? _currentPage = null;
    #endregion

    #region Public Methods
    public void Initialize(IServiceProvider _)
    {
        _currentPage = navigationManager.GetRelativeUrl();
    }

    public void Dispose()
    {
    }
    #endregion

    #region Command Handlers
    public Task<WrappedResponse<string>> Handle(CurrentUrlQuery query, CancellationToken token) =>
        Task.FromResult(WrappedResponse<string>.CreateSuccessResponse(navigationManager.GetRelativeUrl()));

    public async Task Handle(NavigationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Action}]: {Url}", command.Action, command.PageUrl);

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
        if (_currentPage == command.PageUrl) return;

        // add the current page to the stack
        if (_currentPage != null) _history.Push(_currentPage);

        // now perform the navigation
        await Task.Run(() => navigationManager.NavigateTo(command.PageUrl), cancellationToken);

        // publish the close (if possible), then the open
        if (_currentPage != null)
            pubSubService.Publish(new NavigationChange() { Action = NavigationAction.Close, PageUrl = _currentPage });
        pubSubService.Publish(new NavigationChange() { Action = NavigationAction.Open, PageUrl = command.PageUrl });

        // switch to the new page
        _currentPage = command.PageUrl;
    }
    private async Task HandleClose(NavigationCommand command, CancellationToken cancellationToken)
    {
        // if we're not currently on the page being closed, all we need to do is say it is closed
        var currentUrl = _currentPage;
        if (currentUrl != command.PageUrl)
            pubSubService.Publish(new NavigationChange() { Action = NavigationAction.Close, PageUrl = command.PageUrl });

        else
        {
            // get the most recent one from the stack
            if (!_history.TryPop(out var lastPage)) lastPage = "home";
        
            // close the existing by going to the old one
            await Task.Run(() => navigationManager.NavigateTo(lastPage), cancellationToken);
        
            pubSubService.Publish(new NavigationChange() { Action = NavigationAction.Close, PageUrl = command.PageUrl });
            pubSubService.Publish(new NavigationChange() { Action = NavigationAction.Open, PageUrl = lastPage });

            _currentPage = lastPage;
        }
    }
    #endregion
}