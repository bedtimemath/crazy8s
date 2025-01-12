using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Pages;

public class PagesService(
    ILoggerFactory loggerFactory,
    NavigationManager navigationManager): IRequestHandler<GoToPageCommand>
{
    private readonly ILogger<PagesService> _logger = loggerFactory.CreateLogger<PagesService>();
    
    public Task Handle(GoToPageCommand goToPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GoTo: {PageName} [{IdValue}]", goToPageCommand.PageName, goToPageCommand.IdValue);
        
        navigationManager.NavigateTo($"/{goToPageCommand.PageName}/{goToPageCommand.IdValue}");
        return Task.CompletedTask;
    }
}