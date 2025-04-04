using System.Net.Http.Json;
using System.Text.Json;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Callbacks;

public abstract class BaseCallbacks(
    IHttpClientFactory httpClientFactory) : ICallbacks
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    protected async Task<WrappedListResponse<TResult>> CallBackendReturnList<TResult>(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
        where TResult: class?
    {
        var httpResponse = await CallBackendInternal(httpMethod, endpoint, id?.ToString(), payload, token);
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"Network error: [{(int)httpResponse.StatusCode}] {httpResponse.ReasonPhrase}");

        var bodyJson = await httpResponse.Content.ReadAsStringAsync(token);
        var backendResponse = JsonSerializer
                                  .Deserialize<WrappedListResponse<TResult>>(bodyJson, _options) ??
                              throw new Exception($"Could not deserialize: {bodyJson}");

        return backendResponse;
    }

    protected Task<WrappedResponse<TResult>> CallBackendReturnSingle<TResult>(
        string httpMethod, string endpoint, 
        object? payload = null, CancellationToken token = default)
        where TResult : class? =>
        CallBackendReturnSingle<TResult>(httpMethod, endpoint, (string?)null, payload, token);

    protected Task<WrappedResponse<TResult>> CallBackendReturnSingle<TResult>(
        string httpMethod, string endpoint, long? id = null,
        object? payload = null, CancellationToken token = default)
        where TResult : class? =>
        CallBackendReturnSingle<TResult>(httpMethod, endpoint, id?.ToString(), payload, token);

    protected Task<WrappedResponse<TResult>> CallBackendReturnSingle<TResult>(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
        where TResult : class? =>
        CallBackendReturnSingle<TResult>(httpMethod, endpoint, id?.ToString(), payload, token);

    protected async Task<WrappedResponse<TResult>> CallBackendReturnSingle<TResult>(
        string httpMethod, string endpoint, string? idString = null,
        object? payload = null, CancellationToken token = default)
        where TResult: class?
    {
        var httpResponse = await CallBackendInternal(httpMethod, endpoint, idString, payload, token);
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"Network error: [{(int)httpResponse.StatusCode}] {httpResponse.ReasonPhrase}");

        var bodyJson = await httpResponse.Content.ReadAsStringAsync(token);
        var backendResponse = JsonSerializer
                                  .Deserialize<WrappedResponse<TResult>>(bodyJson, _options) ??
                              throw new Exception($"Could not deserialize: {bodyJson}");

        return backendResponse;
    }

    protected async Task<WrappedResponse> CallBackendNoReturn(
        string httpMethod, string endpoint, int? id = null,
        object? payload = null, CancellationToken token = default)
    {
        var httpResponse = await CallBackendInternal(httpMethod, endpoint, id?.ToString(), payload, token);
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"Network error: [{(int)httpResponse.StatusCode}] {httpResponse.ReasonPhrase}");

        var bodyJson = await httpResponse.Content.ReadAsStringAsync(token);
        var backendResponse = JsonSerializer
                                  .Deserialize<WrappedResponse>(bodyJson, _options) ??
                              throw new Exception($"Could not deserialize: {bodyJson}");

        return backendResponse;
    }

    private async Task<HttpResponseMessage> CallBackendInternal(
        string httpMethod, string endpoint, string? idString = null,
        object? payload = null, CancellationToken token = default)
    {
        var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);

        var url = $"api/{endpoint}" + ((idString != null) ? $"/{idString}" : null);
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