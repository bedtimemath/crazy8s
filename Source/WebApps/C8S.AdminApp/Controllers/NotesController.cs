using System.Text.Json;
using AutoMapper;
using C8S.AdminApp.Hubs;
using C8S.AdminApp.Services;
using C8S.Domain;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Commands;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SC.Common;
using SC.Common.Extensions;
using SC.Common.PubSub;
using SC.Common.Responses;

namespace C8S.AdminApp.Controllers;

[ApiController]
public class NotesController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IHubContext<CommunicationHub> hubContext,
    ISelfService selfService) : ControllerBase
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NotesController> _logger = loggerFactory.CreateLogger<NotesController>();
    #endregion

    #region GET LIST
    [HttpPost]
    [Route("api/[controller]")]
    public async Task<WrappedListResponse<NoteDetails>> GetNotes(
        [FromBody] NotesListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            IQueryable<NoteDb> queryable = query.NotesSource switch
            {
                NoteReference.Club => dbContext.ClubNotes.Where(n => n.ClubId == query.SourceId).AsNoTracking(),
                NoteReference.Invoice => dbContext.InvoiceNotes.Where(n => n.InvoiceId == query.SourceId).AsNoTracking(),
                NoteReference.Person => dbContext.PersonNotes.Where(n => n.PersonId == query.SourceId).AsNoTracking(),
                NoteReference.Place => dbContext.PlaceNotes.Where(n => n.PlaceId == query.SourceId).AsNoTracking(),
                NoteReference.Request => dbContext.RequestNotes.Where(n => n.RequestId == query.SourceId).AsNoTracking(),
                NoteReference.Sale => dbContext.SaleNotes.Where(n => n.SaleId == query.SourceId).AsNoTracking(),
                NoteReference.Order => dbContext.OrderNotes.Where(n => n.OrderId == query.SourceId).AsNoTracking(),
                _ => throw new ArgumentOutOfRangeException(nameof(query.NotesSource))
            };

            // sorting is always the same
            queryable = queryable.OrderByDescending(n => n.CreatedOn);

            var items = await mapper.ProjectTo<NoteDetails>(queryable).ToListAsync();
            var total = await queryable.CountAsync();

            return WrappedListResponse<NoteDetails>.CreateSuccessResponse(items, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<NoteDetails>.CreateFailureResponse(exception);
        }
    }
    #endregion

    #region GET SINGLE
    [HttpGet]
    [Route("api/[controller]/{noteId:int}")]
    public async Task<WrappedResponse<NoteDetails?>> GetNote(int noteId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Notes
                .AsSingleQuery()
                .AsNoTracking();

            var note = await queryable.FirstOrDefaultAsync(r => r.NoteId == noteId);

            return new WrappedResponse<NoteDetails?>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return new WrappedResponse<NoteDetails?>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    }
    #endregion

    #region PUT
    [HttpPut]
    [Route("api/[controller]")]
    public async Task<WrappedResponse<NoteDetails>> PutNote(
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
                DataChangeAction = DataChangeAction.Added,
                JsonDetails = JsonSerializer.Serialize(addedDetails)
            };
            await hubContext.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange, dataChange);

            return new WrappedResponse<NoteDetails>()
            {
                Result = addedDetails
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding note: {Json}", JsonSerializer.Serialize(command));
            return new WrappedResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    }
    #endregion

    #region PATCH
    [HttpPatch]
    [Route("api/[controller]/{noteId:int}")]
    public async Task<WrappedResponse<NoteDetails>> PatchNote(int noteId,
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
                DataChangeAction = DataChangeAction.Modified,
                JsonDetails = JsonSerializer.Serialize(mapper.Map<NoteDetails>(note))
            };
            await hubContext.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange, dataChange);

            return new WrappedResponse<NoteDetails>()
            {
                Result = mapper.Map<NoteDetails?>(note)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while patching note: {Id}", noteId);
            return new WrappedResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    } 
    #endregion

    #region DELETE
    [HttpDelete]
    [Route("api/[controller]/{noteId:int}")]
    public async Task<WrappedResponse> DeleteNote(int noteId)
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
                DataChangeAction = DataChangeAction.Deleted,
                JsonDetails = JsonSerializer.Serialize(removedDetails)
            };
            await hubContext.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange, dataChange);

            return WrappedResponse.CreateSuccessResponse();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", noteId);
            return WrappedResponse.CreateFailureResponse(exception);
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
}
