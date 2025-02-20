﻿using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class AppointmentCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<AppointmentDetailsQuery, WrappedResponse<AppointmentDetails?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedResponse<AppointmentDetails?>> Handle(
        AppointmentDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<AppointmentDetails?>("GET", "appointment", query.AppointmentId, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling appointment: {@Appointment}", query);
            return WrappedResponse<AppointmentDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion
}