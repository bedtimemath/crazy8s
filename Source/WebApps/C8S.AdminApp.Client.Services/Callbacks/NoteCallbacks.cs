using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class NoteCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<NotesListQuery, BackendResponse<NotesListResults>>,
        ICQRSQueryHandler<NoteDetailsQuery, BackendResponse<NoteDetails?>>,
        // COMMANDS
        ICQRSCommandHandler<NoteAddCommand, BackendResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteUpdateCommand, BackendResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteDeleteCommand, BackendResponse>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteCallbacks> _logger = loggerFactory.CreateLogger<NoteCallbacks>();
    #endregion

    #region Queries
    public async Task<BackendResponse<NotesListResults>> Handle(
        NotesListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<NotesListResults>("POST", "notes", payload:query, token:token);
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
            return await CallBackendServer<NoteDetails?>("GET", "note", query.NoteId, token:token);
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
            return await CallBackendServer<NoteDetails>("PUT", "notes", payload:command, token:token);
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
            return await CallBackendServer<NoteDetails>("PATCH", "note", command.NoteId, 
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
            return await CallBackendServer("DELETE", "note", command.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note delete command: {@Note}", command);
            return BackendResponse.CreateFailureResponse(exception);
        }
    }
    #endregion
}