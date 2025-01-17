using System.Diagnostics;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Appointments;

public class AppointmentDisplayerCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentDisplayerCoordinator> _logger = loggerFactory.CreateLogger<AppointmentDisplayerCoordinator>();
    #endregion

    #region Public Events
    public event EventHandler? DetailsUpdated;
    public void RaiseDetailsUpdated() => DetailsUpdated?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Private Variables
    private int _requestId;
    private int? _appointmentId;
    private DateTimeOffset? _startsOn;
    #endregion

    #region Public Properties
    public DateTimeOffset? StartsOn => _startsOn;
    #endregion

    #region Public Methods
    public void SetDetailsId(int requestId, int? appointmentId, DateTimeOffset? startsOn)
    {
        _requestId = requestId;
        _appointmentId = appointmentId;
        _startsOn = startsOn;

        if (_startsOn == null && _appointmentId.HasValue)
            Task.Run(CheckAndUpdateAppointment);
    }
    public async Task SetDetailsIdAsync(int requestId, int? appointmentId, DateTimeOffset? startsOn)
    {
        _requestId = requestId;
        _appointmentId = appointmentId;
        _startsOn = startsOn;

        if (_startsOn == null && _appointmentId.HasValue)
            await CheckAndUpdateAppointment().ConfigureAwait(false);
    }
    #endregion

    #region Private Methods
    private async Task CheckAndUpdateAppointment()
    {
        try
        {
            if (!_appointmentId.HasValue)
                throw new UnreachableException("CheckAndUpdateAppointment called with null appointment id");

            var backendResponse = await mediator.Send(
                new AppointmentDetailsQuery() { AppointmentId = _appointmentId.Value });
            if (!backendResponse.Success)
                throw backendResponse.Exception!.ToException();

            var details = backendResponse.Result;
            _startsOn = details?.StartsOn;

            try
            {
                // attempt to update the database so we don't have to always check
                var response = await mediator.Send(new RequestUpdateAppointmentCommand()
                {
                    RequestId = _requestId,
                    FullSlateAppointmentStartsOn = _startsOn
                });
                if (!response.Success)
                    throw response.Exception!.ToException();
            }
            catch (Exception exception)
            {
                // we will log the error, but it doesn't really matter if we can't update
                _logger.LogError(exception, 
                    "Could not update appointment starts on to {StartsOn} for request #{RequestId}", _startsOn, _requestId);
            }

            RaiseDetailsUpdated();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while setting details id.");
            throw; // todo: what happens with exception in controller?
        }
    }
    #endregion

}