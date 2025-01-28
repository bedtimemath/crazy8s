using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Services;
using SC.Common.PubSub;

namespace C8S.AdminApp.Client.Services.Pages;

public sealed class PagesService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
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
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageUrl, openPageCommand.IdValue);

        var newUrl = openPageCommand.PageUrl;
        if (openPageCommand.IdValue != null) newUrl += $"/{openPageCommand.IdValue}";
        var currentUrl = GetCurrentUrl();

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = newUrl,
            Action = PageChangeAction.Open,
            IdValue = openPageCommand.IdValue
        });

        await Task.Run(() => navigationManager.NavigateTo(newUrl), cancellationToken);
    }

    public async Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageUrl, closePageCommand.IdValue);

        var currentUrl = GetCurrentUrl();
        if (currentUrl != closePageCommand.PageUrl) return;
        
        // todo: drop back correctly
        var newUrl = AdminAppConstants.Pages.RequestsList;

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = newUrl,
            Action = PageChangeAction.Close,
            IdValue = closePageCommand.IdValue
        });

        await Task.Run(() => navigationManager.NavigateTo(newUrl), cancellationToken);
    }
    #endregion

    #region Private Methods
    private string GetCurrentUrl() => navigationManager.ToBaseRelativePath(navigationManager.Uri);
    #endregion
}