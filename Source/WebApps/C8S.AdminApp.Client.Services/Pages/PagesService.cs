using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Pages;

public sealed class PagesService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager) : 
    IRequestHandler<OpenPageCommand>, 
    IRequestHandler<ClosePageCommand>, 
    IPagesService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PagesService> _logger = loggerFactory.CreateLogger<PagesService>();
    #endregion

    #region Public Events
    public event EventHandler<PageChangedEventArgs>? PageChanged;
    private void RaisePageChanged(PageChangedAction action, string oldUrl, string newUrl) =>
        PageChanged?.Invoke(this, new PageChangedEventArgs() { Action = action, OldUrl = oldUrl, NewUrl = newUrl});
    #endregion

    #region Command Handlers
    public Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageUrlKey, openPageCommand.IdValue);

        var pageUrl = GetUrlForCommand(openPageCommand);
        RaisePageChanged(PageChangedAction.Opened, GetCurrentUrl(), pageUrl);
        navigationManager.NavigateTo(pageUrl);

        return Task.CompletedTask;
    }

    public Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageUrlKey, closePageCommand.IdValue);

        var pageUrl = GetUrlForCommand(closePageCommand);

        // note that we need to include the root / to compare
        var currentUrl = GetCurrentUrl();
        var newUrl = AdminAppConstants.Pages.RequestsList;
        if (currentUrl == pageUrl)
        {
            RaisePageChanged(PageChangedAction.Closed, currentUrl, newUrl);
            navigationManager.NavigateTo(newUrl);
        }

        return Task.CompletedTask;
    }
    #endregion

    #region Private Methods
    private string GetCurrentUrl() => $"/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}";
    private static string GetUrlForCommand(PageCommand pageCommand) => $"/{pageCommand.PageUrlKey}/{pageCommand.IdValue}"; 
    #endregion
}