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
        ICQRSQueryHandler<NotesListQuery, DomainResponse<NotesListResults>>,
        ICQRSQueryHandler<NoteDetailsQuery, DomainResponse<NoteDetails?>>,
        // COMMANDS
        ICQRSCommandHandler<NoteAddCommand, DomainResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteUpdateCommand, DomainResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteDeleteCommand, DomainResponse>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteCallbacks> _logger = loggerFactory.CreateLogger<NoteCallbacks>();
    #endregion

    #region Queries
    public async Task<DomainResponse<NotesListResults>> Handle(
        NotesListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<NotesListResults>("POST", "notes", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling notes list request: {@Note}", query);
            return DomainResponse<NotesListResults>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<DomainResponse<NoteDetails?>> Handle(
        NoteDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<NoteDetails?>("GET", "note", query.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note details request: {@Note}", query);
            return DomainResponse<NoteDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<DomainResponse<NoteDetails>> Handle(
        NoteAddCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendServer<NoteDetails>("PUT", "notes", payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note add command: {@Note}", command);
            return DomainResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<DomainResponse<NoteDetails>> Handle(
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
            return DomainResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<DomainResponse> Handle(
        NoteDeleteCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendServer("DELETE", "note", command.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note delete command: {@Note}", command);
            return DomainResponse.CreateFailureResponse(exception);
        }
    }
    #endregion
}