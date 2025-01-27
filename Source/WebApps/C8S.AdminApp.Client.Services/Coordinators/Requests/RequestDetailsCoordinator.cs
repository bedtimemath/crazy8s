using C8S.AdminApp.Client.Services.Pages;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Requests;

public sealed class RequestDetailsCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator,
    NavigationManager navigationManager)
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
            var backendResponse = await mediator.Send(
                new RequestDetailsQuery() { RequestId = id });
            if (!backendResponse.Success) 
                throw backendResponse.Exception!.ToException();

            if (backendResponse.Result == null)
            {
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
        => await mediator.Send(new ClosePageCommand()
        {
            PageUrlKey = AdminAppConstants.Pages.RequestDetails,
            IdValue = Details?.RequestId
        });

    public Task LinkPlace()
    {
        throw new NotImplementedException();
    }

    public Task UnlinkPlace()
    {
        throw new NotImplementedException();
    }

    public Task LinkPerson()
    {
        throw new NotImplementedException();
    }

    public Task UnlinkPerson()
    {
        throw new NotImplementedException();
    }

    #endregion
}
