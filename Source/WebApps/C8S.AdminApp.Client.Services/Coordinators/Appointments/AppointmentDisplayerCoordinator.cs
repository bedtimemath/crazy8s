using System.Diagnostics;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Coordinators.Appointments;

public class AppointmentDisplayerCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService)
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

    private CancellationTokenSource? _cancellationTokenSource = null;
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
        {
            Task.Run(CheckAndUpdateAppointment);
        }
    }
    #endregion

    #region Constructors Destructors
    ~AppointmentDisplayerCoordinator()
    {
        if (_cancellationTokenSource == null) return;

        try { _cancellationTokenSource.Cancel(); }
        catch
        {
            // we don't want this to break us
        }
    }
    #endregion

    #region Private Methods
    // todo: this should either wait or have user interaction required
    //  reason - when scrolling quickly, the coordinator may be disposed before the result returns
    //  unfortunately, including a cancel token doesn't always work.
    private async Task CheckAndUpdateAppointment()
    {
        try
        {
            if (!_appointmentId.HasValue)
                throw new UnreachableException("CheckAndUpdateAppointment called with null appointment id");

            var appointmentResponse = await SendCancellable
                <AppointmentDetailsQuery, BackendResponse<AppointmentDetails?>> (
                    new AppointmentDetailsQuery()
                    {
                        AppointmentId = _appointmentId.Value
                    });

            if (!appointmentResponse.Success)
                throw appointmentResponse.Exception!.ToException();

            var details = appointmentResponse.Result;
            _startsOn = details?.StartsOn;

            // attempt to update the database so we don't have to always check
            try
            {
                var databaseResponse = await SendCancellable
                    <RequestUpdateAppointmentCommand, BackendResponse<RequestDetails>> (
                        new RequestUpdateAppointmentCommand()
                        {
                            RequestId = _requestId,
                            FullSlateAppointmentStartsOn = _startsOn
                        });

                if (!databaseResponse.Success)
                    throw databaseResponse.Exception!.ToException();
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

    private async Task<TResult> SendCancellable<TQuery, TResult>(TQuery query)
        where TQuery: class, ICQRSQuery<TResult>
        where TResult : class, new()
   {
        TResult result;
        using (_cancellationTokenSource = new CancellationTokenSource())
        {
            result = (await cqrsService.ExecuteQuery<TQuery, TResult>(query, _cancellationTokenSource.Token)) ??
                     throw new UnreachableException("Could not convert to TResult");
        }
        _cancellationTokenSource = null;
        return result!;
    }
    #endregion

}