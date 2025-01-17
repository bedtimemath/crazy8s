using System.Net.Http.Json;
using System.Text.Json;
using C8S.AdminApp.Common;
using C8S.Domain.Features.Appointments.Models;
using C8S.Domain.Features.Appointments.Queries;
using C8S.Domain.Features.Requests.Commands;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class RequestCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : 
        IRequestHandler<RequestsListQuery, BackendResponse<RequestListResults>>,
        IRequestHandler<RequestDetailsQuery, BackendResponse<RequestDetails?>>,
        IRequestHandler<RequestUpdateAppointmentCommand, BackendResponse<RequestDetails>>
{
    private readonly ILogger<RequestCallbacks> _logger = loggerFactory.CreateLogger<RequestCallbacks>();

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<BackendResponse<RequestListResults>> Handle(
        RequestsListQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.PostAsJsonAsync("api/requests", query, cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                                      .Deserialize<BackendResponse<RequestListResults>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request: {@Request}", query);
            return BackendResponse<RequestListResults>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<BackendResponse<RequestDetails?>> Handle(
        RequestDetailsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.GetAsync($"api/request/{query.RequestId}", cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                                      .Deserialize<BackendResponse<RequestDetails?>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request: {@Request}", query);
            return BackendResponse<RequestDetails?>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<BackendResponse<RequestDetails>> Handle(
        RequestUpdateAppointmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.PatchAsJsonAsync($"api/request/{command.RequestId}", command, cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                                      .Deserialize<BackendResponse<RequestDetails>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error updating request: {@Command}", command);
            return BackendResponse<RequestDetails>.CreateFailureResponse(exception);
        }
    }
}