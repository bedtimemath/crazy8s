using System.Text.Json;
using AutoMapper;
using C8S.AdminApp.Hubs;
using C8S.Domain;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SC.Audit.Abstractions.Models;
using SC.Common;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]/{noteId:int}")]
[ApiController]
public class NoteController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IHubContext<CommunicationHub> hubContext) : ControllerBase
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NoteController> _logger = loggerFactory.CreateLogger<NoteController>();
    #endregion

    #region Public Methods
    [HttpGet]
    public async Task<BackendResponse<NoteDetails?>> GetNote(int noteId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Notes
                .AsSingleQuery()
                .AsNoTracking();

            var note = await queryable.FirstOrDefaultAsync(r => r.NoteId == noteId);

            return new BackendResponse<NoteDetails?>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return new BackendResponse<NoteDetails?>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    }
    
    [HttpPatch]
    public async Task<BackendResponse<NoteDetails>> PatchNote(int noteId,
        [FromBody] NoteUpdateCommand command)
    {
        try
        {
            // find the existing note
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var note = await dbContext.Notes.FindAsync(noteId) ??
                       throw new Exception($"Note ID #{noteId} does not exist.");

            // not bothering with automapper, since the only thing that's allowed to change
            //  is the content
            note.Content = command.Content;
            await dbContext.SaveChangesAsync();
            
            // tell the world
            var dataChange = new DataChange()
            {
                EntityId = noteId,
                EntityName = C8SConstants.Entities.Note,
                EntityState = EntityState.Modified,
                JsonDetails = JsonSerializer.Serialize(mapper.Map<NoteDetails>(note))
            };
            await hubContext.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange, dataChange);

            return new BackendResponse<NoteDetails>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while patching note: {Id}", noteId);
            return new BackendResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    } 

    [HttpDelete]
    public async Task<BackendResponse> DeleteNote(int noteId)
    {
        try
        {
            // find the existing note
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var note = await dbContext.Notes.FirstOrDefaultAsync(r => r.NoteId == noteId) ??
                       throw new Exception("Entity not found");

            // get the note as details, to return in JsonDetails
            var removedDetails = mapper.Map<NoteDetails>(note);

            // remove the note
            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();

            // tell the world
            var dataChange = new DataChange()
            {
                EntityId = noteId,
                EntityName = C8SConstants.Entities.Note,
                EntityState = EntityState.Deleted,
                JsonDetails = JsonSerializer.Serialize(removedDetails)
            };
            await hubContext.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange, dataChange);

            return BackendResponse.CreateSuccessResponse();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return BackendResponse.CreateFailureResponse(exception);
        }
    }

    #endregion
}