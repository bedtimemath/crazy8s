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
using SC.Common.Interactions;
using SC.Common.PubSub;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
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

    #region Public Methods
    [HttpPost]
    public async Task<DomainResponse<NotesListResults>> GetNotes(
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

            var totalNotes = await queryable.CountAsync();
            var notes = await queryable.ToListAsync();

            return new DomainResponse<NotesListResults>()
            {
                Result = new NotesListResults()
                {
                    Items = notes
                        .Select(mapper.Map<NoteDetails>)
                        .ToList(),
                    Total = totalNotes
                }
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return new DomainResponse<NotesListResults>()
            {
                Exception = exception.ToSerializableException()
            };
        }

    }

    [HttpPut]
    public async Task<DomainResponse<NoteDetails>> PutNote(
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

            return new DomainResponse<NoteDetails>()
            {
                Result = addedDetails
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while adding note: {Json}", JsonSerializer.Serialize(command));
            return new DomainResponse<NoteDetails>()
            {
                Exception = exception.ToSerializableException()
            };
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
