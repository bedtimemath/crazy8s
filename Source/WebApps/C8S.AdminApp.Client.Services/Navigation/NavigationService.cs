using C8S.AdminApp.Client.Services.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public sealed class NavigationService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    NavigationManager navigationManager) : INavigationService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NavigationService> _logger = loggerFactory.CreateLogger<NavigationService>();
    #endregion

    #region Command Handlers
    public async Task Handle(NavigationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Action}]: {Group} [{IdValue}]", command.Action, command.Group, command.IdValue);

        await Task.Run(() => navigationManager.NavigateTo(command.PageUrl), cancellationToken);
        await pubSubService.Publish(new NavigationChange()
        {
            CurrentUrl = navigationManager.GetRelativeUrl(),
            Action = command.Action,
            Group = command.Group,
            IdValue = command.IdValue,
            JsonDetails = command.JsonDetails
        });
    }
    #endregion

#if false // todo
        var newUrl = openNavigationCommand.PageUrl;
        if (openNavigationCommand.IdValue != null) newUrl += $"/{openNavigationCommand.IdValue}";
        var currentUrl = GetCurrentUrl();

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = newUrl,
            Action = PageChangeAction.Open,
            IdValue = openNavigationCommand.IdValue
        });

        await Task.Run(() => navigationManager.NavigateTo(newUrl), cancellationToken);

#endif    
#if false // todo
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
#endif

    public ValueTask HandleLocationChanging(LocationChangingContext context)
    {
        _logger.LogDebug("Context={@Context}", context);
        return ValueTask.CompletedTask;
    }
}