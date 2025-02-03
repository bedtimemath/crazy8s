using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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

    #region Command Handlers
    public async Task Handle(NavigationCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Action}]: {Group} [{IdValue}]", command.Action, command.Entity, command.IdValue);

        // if we're initializing, send out the change, but don't actually navigate; this
        //  allows us to run the command once when the first page is initially opened
        var oldUrl = navigationManager.GetRelativeUrl();
        if (command.Action != NavigationAction.Initialize)
            await Task.Run(() => navigationManager.NavigateTo(command.PageUrl), cancellationToken);
    
        // now let everyone know
        await pubSubService.Publish(new NavigationChange()
        {
            OldUrl = oldUrl,
            Action = command.Action,
            Entity = command.Entity,
            PageUrl = command.PageUrl,
            IdValue = command.IdValue,
            JsonDetails = command.JsonDetails
        });
    }
    #endregion
}