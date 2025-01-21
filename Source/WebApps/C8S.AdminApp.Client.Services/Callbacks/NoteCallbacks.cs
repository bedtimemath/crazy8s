using System.Net.Http.Json;
using System.Text.Json;
using C8S.AdminApp.Common;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class NoteCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : 
        IRequestHandler<NotesListQuery, BackendResponse<NotesListResults>>,
        IRequestHandler<NoteDetailsQuery, BackendResponse<NoteDetails?>>
{
    private readonly ILogger<NoteCallbacks> _logger = loggerFactory.CreateLogger<NoteCallbacks>();

    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    
    public async Task<BackendResponse<NotesListResults>> Handle(
        NotesListQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.PostAsJsonAsync("api/notes", query, cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                                      .Deserialize<BackendResponse<NotesListResults>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling notes list request: {@Note}", query);
            return BackendResponse<NotesListResults>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<BackendResponse<NoteDetails?>> Handle(
        NoteDetailsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            var httpResponse = await httpClient.GetAsync($"api/note/{query.NoteId}", cancellationToken);
            var bodyJson = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var backendResponse = JsonSerializer
                .Deserialize<BackendResponse<NoteDetails?>>(bodyJson, _options) ??
                                  throw new Exception($"Could not deserialize: {bodyJson}");

            return backendResponse;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note details request: {@Note}", query);
            return BackendResponse<NoteDetails?>.CreateFailureResponse(exception);
        }
    }
}