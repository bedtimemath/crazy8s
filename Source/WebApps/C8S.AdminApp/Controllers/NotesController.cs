using System.Linq.Dynamic.Core;
using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Notes.Models;
using C8S.Domain.Features.Notes.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<NotesController> _logger = loggerFactory.CreateLogger<NotesController>();

    [HttpPost]
    public async Task<BackendResponse<NotesListResults>> GetNotes(
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

            var totalNotes = await queryable.CountAsync();
            var notes = await queryable.ToListAsync();

            return new BackendResponse<NotesListResults>()
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
            return new BackendResponse<NotesListResults>()
            {
                Exception = exception.ToSerializableException()
            };
        }

    }
}