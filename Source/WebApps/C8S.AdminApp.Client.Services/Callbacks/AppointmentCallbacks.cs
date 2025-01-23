using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class AppointmentCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        IRequestHandler<AppointmentDetailsQuery, BackendResponse<AppointmentDetails?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Queries
    public async Task<BackendResponse<AppointmentDetails?>> Handle(
        AppointmentDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<AppointmentDetails?>("GET", "appointment", query.AppointmentId, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling appointment: {@Appointment}", query);
            return BackendResponse<AppointmentDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion
}