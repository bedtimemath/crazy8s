using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Services;
using SC.Common.PubSub;

namespace C8S.AdminApp.Client.Services.Pages;

public sealed class PagesService(
    ILoggerFactory loggerFactory,
    PubSubService pubSubService,
    NavigationManager navigationManager) : 
    IRequestHandler<OpenPageCommand>, 
    IRequestHandler<ClosePageCommand>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PagesService> _logger = loggerFactory.CreateLogger<PagesService>();
    #endregion

    #region Public Properties
    public string CurrentUrl
    {
        get
        {
            if (String.IsNullOrWhiteSpace(_currentUrl))
            {
                _logger.LogDebug("Reading from navigation manager");
                _currentUrl =  $"/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}";
            }
            return _currentUrl;
        }
    }
    private string? _currentUrl;
    #endregion

    #region Command Handlers
    public async Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageUrl, openPageCommand.IdValue);

        _currentUrl = openPageCommand.PageUrl;
        navigationManager.NavigateTo(_currentUrl);

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = CurrentUrl,
            NewUrl = openPageCommand.PageUrl,
            Action = PageChangeAction.Open,
            IdValue = openPageCommand.IdValue
        }).ConfigureAwait(false);
    }

    public async Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageUrl, closePageCommand.IdValue);

        // todo: drop back correctly
        var newUrl = AdminAppConstants.Pages.RequestsList;
        if (CurrentUrl != closePageCommand.PageUrl) return;

        _currentUrl = newUrl;
        navigationManager.NavigateTo(_currentUrl);

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = CurrentUrl,
            NewUrl = closePageCommand.PageUrl,
            Action = PageChangeAction.Close,
            IdValue = closePageCommand.IdValue
        }).ConfigureAwait(false);
    }
    #endregion
}