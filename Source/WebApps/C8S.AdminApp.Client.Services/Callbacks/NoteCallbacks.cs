using System.Text.Json;
using C8S.AdminApp.Common;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class NoteCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks,
        // NOTES
        IRequestHandler<NotesListQuery, BackendResponse<NotesListResults>>,
        IRequestHandler<NoteAddCommand, BackendResponse<NoteDetails>>,
        // NOTE
        IRequestHandler<NoteDetailsQuery, BackendResponse<NoteDetails?>>,
        IRequestHandler<NoteUpdateCommand, BackendResponse<NoteDetails>>,
        IRequestHandler<NoteDeleteCommand, BackendResponse>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteCallbacks> _logger = loggerFactory.CreateLogger<NoteCallbacks>();
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
    #endregion

    #region Queries
    public async Task<BackendResponse<NotesListResults>> Handle(
        NotesListQuery query, CancellationToken token)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            return await CallBackendServer<NotesListResults>(httpClient, "POST", "notes", 
                payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling notes list request: {@Note}", query);
            return BackendResponse<NotesListResults>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<BackendResponse<NoteDetails?>> Handle(
        NoteDetailsQuery query, CancellationToken token)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            return await CallBackendServer<NoteDetails?>(httpClient, "GET", "note", query.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note details request: {@Note}", query);
            return BackendResponse<NoteDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<BackendResponse<NoteDetails>> Handle(
        NoteAddCommand command, CancellationToken token)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            return await CallBackendServer<NoteDetails>(httpClient, "PUT", "notes", 
                payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note add command: {@Note}", command);
            return BackendResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<BackendResponse<NoteDetails>> Handle(
        NoteUpdateCommand command, CancellationToken token)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            return await CallBackendServer<NoteDetails>(httpClient, "PATCH", "note", command.NoteId, 
                payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note update command: {@Note}", command);
            return BackendResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<BackendResponse> Handle(
        NoteDeleteCommand command, CancellationToken token)
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(AdminAppConstants.HttpClients.BackendServer);
            return await CallBackendServer(httpClient, "DELETE", "note", command.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note delete command: {@Note}", command);
            return BackendResponse.CreateFailureResponse(exception);
        }
    }
    #endregion
}