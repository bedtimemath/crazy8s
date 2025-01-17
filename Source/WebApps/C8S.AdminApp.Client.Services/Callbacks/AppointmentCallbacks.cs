using System.Net.Http.Json;
using System.Text.Json;
using C8S.AdminApp.Common;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class AppointmentCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : 
        IRequestHandler<AppointmentDetailsQuery, BackendResponse<AppointmentDetails?>>
{
    private readonly ILogger<AppointmentCallbacks> _logger = loggerFactory.CreateLogger<AppointmentCallbacks>();

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<BackendResponse<AppointmentDetails?>> Handle(
        AppointmentDetailsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.GetAsync($"api/appointment/{query.AppointmentId}", cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                .Deserialize<BackendResponse<AppointmentDetails?>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling appointment: {@Appointment}", query);
            return BackendResponse<AppointmentDetails?>.CreateFailureResponse(exception);
        }
    }
}