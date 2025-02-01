﻿using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Navigation.Commands;
using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.AdminApp.Client.Services.Navigation.Models;
using C8S.AdminApp.Client.Services.Navigation.Queries;
using C8S.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
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

        await Task.Run(() => navigationManager.NavigateTo(command.PageUrl), cancellationToken);
        await pubSubService.Publish(new NavigationChange()
        {
            CurrentUrl = navigationManager.GetRelativeUrl(),
            Action = command.Action,
            Entity = command.Entity,
            IdValue = command.IdValue,
            JsonDetails = command.JsonDetails
        });
    }
    
    public Task<DomainResponse<IEnumerable<NavigationGroup>>> Handle(NavigationGroupsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{@Query}", query);
        IEnumerable<NavigationGroup> list = new List<NavigationGroup>()
        {
            new() { Entity = NavigationEntity.Requests, Display = "Requests", IconString = C8SConstants.Icons.Request },
            new() { Entity = NavigationEntity.Contacts, Display = "Contacts", IconString = C8SConstants.Icons.Contact }
        };
        return Task.FromResult(
            DomainResponse<IEnumerable<NavigationGroup>>.CreateSuccessResponse(list));
    }
    #endregion
}