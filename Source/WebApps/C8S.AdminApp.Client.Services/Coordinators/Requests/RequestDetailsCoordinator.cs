using C8S.AdminApp.Client.Services.Pages;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Abstractions.Models;

namespace C8S.AdminApp.Client.Services.Coordinators.Requests;

public sealed class RequestDetailsCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService,
    IPubSubService pubSubService,
    NavigationManager navigationManager): BaseCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestDetailsCoordinator> _logger = loggerFactory.CreateLogger<RequestDetailsCoordinator>();
    #endregion
    
    #region Public Events
    public event EventHandler? DetailsLoaded;
    public void RaiseDetailsLoaded() => DetailsLoaded?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public RequestDetails? Details { get; private set; } 
    #endregion

    #region Public Methods
    public async Task SetIdAsync(int id)
    {
        try
        {
            var backendResponse = await GetQueryResults<RequestDetailsQuery, BackendResponse<RequestDetails?>>
                (new RequestDetailsQuery() { RequestId = id });
            if (!backendResponse.Success)
                throw backendResponse.Exception!.ToException();

            if (backendResponse.Result == null)
            {
                // todo - change once we've got a better page management set up
                navigationManager.NavigateTo(AdminAppConstants.Pages.RequestsList);
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
        => await ExecuteCommand(new ClosePageCommand()
        {
            PageUrl = AdminAppConstants.Pages.RequestDetails,
            IdValue = Details?.RequestId
        });

    public async Task LinkPlace()
    {
        await pubSubService.Publish(new Notification()
        {
            Level = NotificationLevel.Success,
            Detail = "You clicked the link button! Good job."
        })
        .ConfigureAwait(false);
    }

    public Task UnlinkPlace()
    {
        throw new NotImplementedException();
    }

    public async Task LinkPerson()
    {
        await pubSubService.Publish(new Notification()
            {
                Level = NotificationLevel.Error,
                Summary = "Oops! You did that wrong.",
                Detail = "Never click the link person button!!!!"
            })
            .ConfigureAwait(false);
    }

    public Task UnlinkPerson()
    {
        throw new NotImplementedException();
    }

    #endregion
}
