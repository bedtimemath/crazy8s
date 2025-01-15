using System.Net.Http.Json;
using System.Text.Json;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Apis;

public class CallbackService(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : 
        IRequestHandler<RequestsListQuery, BackendResponse<RequestListResults>>,
        IRequestHandler<RequestDetailsQuery, BackendResponse<RequestDetails?>>
{
    private readonly ILogger<CallbackService> _logger = loggerFactory.CreateLogger<CallbackService>();

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<BackendResponse<RequestListResults>> Handle(
        RequestsListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(nameof(CallbackService));
            var httpResponse = await httpClient.PostAsJsonAsync("api/requests", request, cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                .Deserialize<BackendResponse<RequestListResults>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request: {@Request}", request);
            return BackendResponse<RequestListResults>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<BackendResponse<RequestDetails?>> Handle(
        RequestDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(nameof(CallbackService));
            var httpResponse = await httpClient.GetAsync($"api/request/{request.RequestId}", cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                .Deserialize<BackendResponse<RequestDetails?>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling request: {@Request}", request);
            return BackendResponse<RequestDetails?>.CreateFailureResponse(exception);
        }
    }
}