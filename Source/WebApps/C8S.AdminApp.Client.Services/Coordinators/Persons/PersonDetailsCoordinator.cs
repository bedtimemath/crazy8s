using System.Diagnostics;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Navigation.Commands;
using SC.Common.Client.Navigation.Enums;
using SC.Common.Client.Navigation.Queries;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Abstractions.Models;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Persons;

public sealed class PersonDetailsCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService,
    NavigationManager navigationManager) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<PersonDetailsCoordinator> _logger = loggerFactory.CreateLogger<PersonDetailsCoordinator>();
    #endregion
    
    #region Public Events
    public event EventHandler? DetailsLoaded;
    public void RaiseDetailsLoaded() => DetailsLoaded?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public Person? Details { get; private set; } 
    #endregion

    #region Public Methods
    public async Task SetIdAsync(int id)
    {
        try
        {
            var backendResponse = await GetQueryResults<PersonQuery, WrappedResponse<Person?>>
                (new PersonQuery() { PersonId = id });
            if (!backendResponse.Success)
                throw backendResponse.Exception!.ToException();

            if (backendResponse.Result == null)
            {
                // todo - change once we've got a better page management set up
                navigationManager.NavigateTo(AdminAppConstants.Pages.PersonsList);
            }

            Details = backendResponse.Result;

            RaiseDetailsLoaded(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while setting details id.");
            throw; // todo: what happens with exception in coordinator?
        }
    }

    public async Task ClosePage()
    {
        var currentUrl = (await GetQueryResults<CurrentUrlQuery, WrappedResponse<string>>(new CurrentUrlQuery())).Result ??
                         throw new UnreachableException("Could not get a return for the current url.");
        await ExecuteCommand(new NavigationCommand()
        {
            Action = NavigationAction.Close,
            PageUrl = currentUrl
        });
    }

    public async Task LinkPlace()
    {
        PubSubService.Publish(new Notification()
        {
            Level = NotificationLevel.Success,
            Detail = "You clicked the link button! Good job."
        });
    }

    public Task UnlinkPlace()
    {
        throw new NotImplementedException();
    }

    public async Task LinkPerson()
    {
        PubSubService.Publish(new Notification()
            {
                Level = NotificationLevel.Error,
                Summary = "Oops! You did that wrong.",
                Detail = "Never click the link person button!!!!"
            });
    }

    public Task UnlinkPerson()
    {
        throw new NotImplementedException();
    }

    #endregion
}
