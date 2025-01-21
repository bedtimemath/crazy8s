using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]/{noteId:int}")]
[ApiController]
public class NoteController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<NoteController> _logger = loggerFactory.CreateLogger<NoteController>();

    [HttpGet]
    public async Task<BackendResponse<NoteDetails>> GetNote(int noteId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Notes
                .AsSingleQuery()
                .AsNoTracking();

            var note = await queryable.FirstOrDefaultAsync(r => r.NoteId == noteId);

            return new BackendResponse<NoteDetails>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return new BackendResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    }

#if false
    [HttpPatch]
    public async Task<BackendResponse<NoteDetails>> PatchNote(int noteId,
[FromBody] NoteUpdateAppointmentCommand command)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var note = await dbContext.Notes.FindAsync(noteId) ??
                throw new Exception($"Note ID #{noteId} does not exist.");

            note.FullSlateAppointmentStartsOn = command.FullSlateAppointmentStartsOn;
            await dbContext.SaveChangesAsync();

            return new BackendResponse<NoteDetails>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while patching appointment starts on: {Id}", noteId);
            return new BackendResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    } 
#endif
}