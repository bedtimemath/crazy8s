﻿using C8S.AdminApp.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Pages;

public class PagesService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager): IRequestHandler<OpenPageCommand>, IRequestHandler<ClosePageCommand>, IPagesService
{
    private readonly ILogger<PagesService> _logger = loggerFactory.CreateLogger<PagesService>();
    
    public Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Open: {PageName} [{IdValue}]", openPageCommand.PageName, openPageCommand.IdValue);
        
        navigationManager.NavigateTo(GetUrlForCommand(openPageCommand));
        
        return Task.CompletedTask;
    }
    
    public Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Close: {PageName} [{IdValue}]", closePageCommand.PageName, closePageCommand.IdValue);

        // note that we need to include the root /
        if ($"/{navigationManager.ToBaseRelativePath(navigationManager.Uri)}" == GetUrlForCommand(closePageCommand))
            navigationManager.NavigateTo(AdminAppConstants.Pages.RequestList);
            
        return Task.CompletedTask;
    }

    private static string GetUrlForCommand(PageCommand pageCommand) => $"/{pageCommand.PageName}/{pageCommand.IdValue}";
}