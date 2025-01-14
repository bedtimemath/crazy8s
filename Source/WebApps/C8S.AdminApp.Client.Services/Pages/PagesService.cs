using C8S.AdminApp.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Pages;

public sealed class PagesService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager): IRequestHandler<OpenPageCommand>, IRequestHandler<ClosePageCommand>, IPagesService
{
    private readonly ILogger<PagesService> _logger = loggerFactory.CreateLogger<PagesService>();
    
    public Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageUrlKey, openPageCommand.IdValue);

        var pageUrl = GetUrlForCommand(openPageCommand);
        navigationManager.NavigateTo(pageUrl);
        
        return Task.CompletedTask;
    }
    
    public Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageUrlKey, closePageCommand.IdValue);

        var pageUrl = GetUrlForCommand(closePageCommand);
        
        // note that we need to include the root / to compare
        if ($"/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}" == pageUrl)
            navigationManager.NavigateTo(AdminAppConstants.Pages.RequestList);

        return Task.CompletedTask;
    }

    private static string GetUrlForCommand(PageCommand pageCommand) => $"/{pageCommand.PageUrlKey}/{pageCommand.IdValue}";
}