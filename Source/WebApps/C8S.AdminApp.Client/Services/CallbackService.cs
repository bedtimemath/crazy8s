using System.Net.Http.Json;
using System.Text.Json;
using C8S.Domain.Queries;
using C8S.Domain.Queries.List;
using MediatR;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services;

public class CallbackService(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : 
        IRequestHandler<ListApplicationsQuery, ApplicationListResults>
{
    private readonly ILogger<CallbackService> _logger = loggerFactory.CreateLogger<CallbackService>();

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };
    
    public async Task<ApplicationListResults> Handle(
        ListApplicationsQuery request, CancellationToken cancellationToken)
    {
        // todo: check for exceptions when posting
        var httpClient = httpClientFactory.CreateClient(nameof(CallbackService));
        var httpResponse = await httpClient.PostAsJsonAsync("api/applications", request, cancellationToken);
        var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        var backendResponse = JsonSerializer
            .Deserialize<BackendResponse<ApplicationListResults>>(bodyJson, _options);

        return backendResponse!.Result!;
    }
}