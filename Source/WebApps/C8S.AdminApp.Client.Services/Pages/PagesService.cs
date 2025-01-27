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

    #region Command Handlers
    public async Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageUrlKey, openPageCommand.IdValue);

        var newUrl = GetUrlForCommand(openPageCommand);

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = GetCurrentUrl(),
            NewUrl = newUrl,
            Action = PageChangeAction.Open,
            IdValue = openPageCommand.IdValue
        });

        navigationManager.NavigateTo(newUrl);
    }

    public async Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageUrlKey, closePageCommand.IdValue);

        var pageUrl = GetUrlForCommand(closePageCommand);

        // note that we need to include the root / to compare
        var currentUrl = GetCurrentUrl();
        var newUrl = AdminAppConstants.Pages.RequestsList;
        if (currentUrl != pageUrl) return;

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = pageUrl,
            Action = PageChangeAction.Close,
            IdValue = closePageCommand.IdValue
        });

        navigationManager.NavigateTo(newUrl);
    }
    #endregion

    #region Private Methods
    private string GetCurrentUrl() => $"/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}";
    private static string GetUrlForCommand(PageCommand pageCommand) => $"/{pageCommand.PageUrlKey}/{pageCommand.IdValue}"; 
    #endregion
}