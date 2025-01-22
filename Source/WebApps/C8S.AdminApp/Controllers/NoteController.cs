using System.Text.Json;
using AutoMapper;
using C8S.AdminApp.Auth;
using C8S.AdminApp.Common;
using C8S.AdminApp.Hubs;
using C8S.Domain;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SC.Audit.Abstractions.Models;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]/{noteId:int?}")]
[ApiController]
public class NoteController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IHubContext<CommunicationHub> hubContext,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    ISelfService selfService) : ControllerBase
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

    [HttpPut]
    public async Task<BackendResponse<NoteDetails>> PutNote(
        [FromBody] NoteAddCommand command)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var addedDetails = command.Reference switch
            {
                NoteReference.Club => await AddClubNote(dbContext, command),
                NoteReference.Invoice => await AddInvoiceNote(dbContext, command),
                NoteReference.Person => await AddPersonNote(dbContext, command),
                NoteReference.Place => await AddPlaceNote(dbContext, command),
                NoteReference.Request => await AddRequestNote(dbContext, command),
                NoteReference.Sale => await AddSaleNote(dbContext, command),
                NoteReference.Order => await AddOrderNote(dbContext, command),
                _ => throw new ArgumentOutOfRangeException(nameof(NoteAddCommand.Reference))
            };

            var dataChange = new DataChange()
            {
                EntityId = addedDetails.NoteId,
                EntityName = C8SConstants.Entities.Note,
                EntityState = EntityState.Added,
                JsonDetails = JsonSerializer.Serialize(addedDetails)
            };
            await hubContext.Clients.All.SendAsync(AdminAppConstants.Messages.DataChange, dataChange);

            return new BackendResponse<NoteDetails>()
            {
                Result = addedDetails
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding note: {Json}", JsonSerializer.Serialize(command));
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

            var dataChange = new DataChange()
            {
                EntityId = noteId,
                EntityName = C8SConstants.Entities.Note,
                EntityState = EntityState.Deleted,
                JsonDetails = JsonSerializer.Serialize(removedDetails)
            };
            await hubContext.Clients.All.SendAsync(AdminAppConstants.Messages.DataChange, dataChange);

            return BackendResponse.CreateSuccessResponse();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return BackendResponse.CreateFailureResponse(exception);
        }
    }

    #endregion

    #region Private Helper Methods
    private async Task<NoteDetails> AddClubNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new ClubNoteDb()
        {
            Reference = command.Reference,
            ClubId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.ClubNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    }

    private async Task<NoteDetails> AddInvoiceNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new InvoiceNoteDb()
        {
            Reference = command.Reference,
            InvoiceId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.InvoiceNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    }

    private async Task<NoteDetails> AddPersonNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new PersonNoteDb()
        {
            Reference = command.Reference,
            PersonId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.PersonNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    }

    private async Task<NoteDetails> AddPlaceNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new PlaceNoteDb()
        {
            Reference = command.Reference,
            PlaceId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.PlaceNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    }

    private async Task<NoteDetails> AddRequestNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new RequestNoteDb()
        {
            Reference = command.Reference,
            RequestId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.RequestNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    }

    private async Task<NoteDetails> AddSaleNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new SaleNoteDb()
        {
            Reference = command.Reference,
            SaleId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.SaleNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    } 

    private async Task<NoteDetails> AddOrderNote(C8SDbContext dbContext, NoteAddCommand command)
    {
        var noteDb = new OrderNoteDb()
        {
            Reference = command.Reference,
            OrderId = command.ParentId,
            Content = command.Content,
            Author = selfService.DisplayName
        };
        dbContext.OrderNotes.Add(noteDb);
        await dbContext.SaveChangesAsync();
        return mapper.Map<NoteDetails>(noteDb);
    } 
    #endregion


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