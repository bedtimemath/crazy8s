using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using Microsoft.Extensions.Logging;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Callbacks;

public class NoteCallbacks(
    ILoggerFactory loggerFactory,
    IHttpClientFactory httpClientFactory) : BaseCallbacks(httpClientFactory),
        // QUERIES
        ICQRSQueryHandler<NotesListQuery, WrappedListResponse<NoteDetails>>,
        ICQRSQueryHandler<NoteDetailsQuery, WrappedResponse<NoteDetails?>>,
        // COMMANDS
        ICQRSCommandHandler<NoteAddCommand, WrappedResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteUpdateCommand, WrappedResponse<NoteDetails>>,
        ICQRSCommandHandler<NoteDeleteCommand, WrappedResponse>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteCallbacks> _logger = loggerFactory.CreateLogger<NoteCallbacks>();
    #endregion

    #region Queries
    public async Task<WrappedListResponse<NoteDetails>> Handle(
        NotesListQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnList<NoteDetails>("POST", "notes", payload:query, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling notes list request: {@Note}", query);
            return WrappedListResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }
    
    public async Task<WrappedResponse<NoteDetails?>> Handle(
        NoteDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<NoteDetails?>("GET", "notes", query.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note details request: {@Note}", query);
            return WrappedResponse<NoteDetails?>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region Commands
    public async Task<WrappedResponse<NoteDetails>> Handle(
        NoteAddCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<NoteDetails>("PUT", "notes", payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note add command: {@Note}", command);
            return WrappedResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse<NoteDetails>> Handle(
        NoteUpdateCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendReturnSingle<NoteDetails>("PATCH", "notes", command.NoteId, 
                payload:command, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note update command: {@Note}", command);
            return WrappedResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }

    public async Task<WrappedResponse> Handle(
        NoteDeleteCommand command, CancellationToken token)
    {
        try
        {
            return await CallBackendNoReturn("DELETE", "notes", command.NoteId, token:token);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error handling note delete command: {@Note}", command);
            return WrappedResponse.CreateFailureResponse(exception);
        }
    }
    #endregion
}