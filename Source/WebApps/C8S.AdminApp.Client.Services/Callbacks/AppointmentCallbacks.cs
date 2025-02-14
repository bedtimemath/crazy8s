﻿using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class AppointmentCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<AppointmentDetailsQuery, DomainResponse<AppointmentDetails?>>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();
    #endregion

    #region Queries
    public async Task<DomainResponse<AppointmentDetails?>> Handle(
        AppointmentDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<AppointmentDetails?>("GET", "appointment", query.AppointmentId, token: token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling appointment: {@Appointment}", query);
            return DomainResponse<AppointmentDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion
}