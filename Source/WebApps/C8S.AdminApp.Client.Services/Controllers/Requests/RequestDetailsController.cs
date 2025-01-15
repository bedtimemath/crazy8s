using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Controllers.Requests;

public sealed class RequestDetailsController(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region Constants & ReadOnlys
    #endregion

    #region ReadOnly Constructor Variables
    private readonly ILogger<RequestDetailsController> _logger = loggerFactory.CreateLogger<RequestDetailsController>();
    #endregion

    #region ReadOnly Variables
    #endregion

    #region Public Events
    public event EventHandler? DetailsUpdated;
    public void RaiseDetailsUpdated() => DetailsUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties
    public string? PageTitle { get; set; }
    public RequestDetails? Details { get; private set; } 
    #endregion

    #region Public Methods

    public void SetDetailsId(int id)
    {
        Task.Run(async () => await SetDetailsIdAsync(id));
    }
    public async Task SetDetailsIdAsync(int id)
    {
        try
        {
            var backendResponse = await mediator.Send(
                new RequestDetailsQuery() { RequestId = id });
            if (!backendResponse.Success) 
                throw backendResponse.Exception!.ToException();
            
            Details = backendResponse.Result;
            PageTitle = Details?.PersonFullName;
            RaiseDetailsUpdated();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while setting details id.");
            throw; // todo: what happens with exception in controller?
        }
    }
    #endregion

    #region Event Handlers
    #endregion
}
