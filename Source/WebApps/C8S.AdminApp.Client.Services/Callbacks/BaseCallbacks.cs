using System.Net.Http.Json;
using System.Text.Json;
using C8S.AdminApp.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public abstract class BaseCallbacks(
    IHttpClientFactory httpClientFactory)
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    protected async Task<BackendResponse<TResult>> CallBackendServer<TResult>(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
        where TResult: class?
    {
        var httpResponse = await CallBackendInternal(httpMethod, endpoint, id, payload, token);
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"Network error: [{(int)httpResponse.StatusCode}] {httpResponse.ReasonPhrase}");

        var bodyJson = await httpResponse.Content.ReadAsStringAsync(token);
        var backendResponse = JsonSerializer
                                  .Deserialize<BackendResponse<TResult>>(bodyJson, _options) ??
                              throw new Exception($"Could not deserialize: {bodyJson}");

        return backendResponse;
    }

    protected async Task<BackendResponse> CallBackendServer(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
    {
        var httpResponse = await CallBackendInternal(httpMethod, endpoint, id, payload, token);
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"Network error: [{(int)httpResponse.StatusCode}] {httpResponse.ReasonPhrase}");

        var bodyJson = await httpResponse.Content.ReadAsStringAsync(token);
        var backendResponse = JsonSerializer
                                  .Deserialize<BackendResponse>(bodyJson, _options) ??
                              throw new Exception($"Could not deserialize: {bodyJson}");

        return backendResponse;
    }

    private async Task<HttpResponseMessage> CallBackendInternal(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
    {
        var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);

        var url = $"api/{endpoint}" + ((id != null) ? $"/{id}" : null);
        return httpMethod switch
        {
            "GET" => await httpClient.GetAsync(url, token),
            "POST" => await httpClient.PostAsJsonAsync(url, payload, token),
            "PUT" => await httpClient.PutAsJsonAsync(url, payload, token),
            "PATCH" => await httpClient.PatchAsJsonAsync(url, payload, token),
            "DELETE" => await httpClient.DeleteAsync(url, token),
            _ => throw new ArgumentOutOfRangeException(nameof(httpMethod))
        };
    }
}