using System.Linq.Dynamic.Core;
using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Responses;

namespace C8S.AdminApp.Controllers;

[ApiController]
public class PersonsController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<PersonsController> _logger = loggerFactory.CreateLogger<PersonsController>();

    #region GET LIST
    [HttpPost]
    [Route("api/[controller]")]
    public async Task<WrappedListResponse<PersonListItem>> GetPersons(
    [FromBody] PersonsListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .AsSingleQuery()
                .AsNoTracking();

            /*** QUERY ***/
            if (!String.IsNullOrEmpty(query.Query))
            {
                queryable = queryable.Where(r => (!String.IsNullOrEmpty(r.FirstName) && r.FirstName.Contains(query.Query)) ||
                                                 r.LastName.Contains(query.Query) ||
                                                 r.Email.Contains(query.Query));
            }

            /*** SORT ***/
            if (!String.IsNullOrEmpty(query.SortDescription))
                queryable = queryable.OrderBy(query.SortDescription);

            var totalPersons = await queryable.CountAsync();

            if (query.StartIndex != null) queryable = queryable.Skip(query.StartIndex.Value);
            if (query.Count != null) queryable = queryable.Take(query.Count.Value);

            var items = await mapper.ProjectTo<PersonListItem>(queryable).ToListAsync();
            var total = await queryable.CountAsync();

            return WrappedListResponse<PersonListItem>.CreateSuccessResponse(items, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<PersonListItem>.CreateFailureResponse(exception);
        }

    } 
    #endregion

    #region GET SINGLE
    [HttpGet]
    [Route("api/[controller]/{personId:int}")]
    public async Task<WrappedResponse<PersonDetails>> GetPerson(int personId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .AsSingleQuery()
                .AsNoTracking();

            var person = await queryable.FirstOrDefaultAsync(r => r.PersonId == personId);

            return new WrappedResponse<PersonDetails>()
            {
                Result = mapper.Map<PersonDetails?>(person)
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", personId);
            return new WrappedResponse<PersonDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }
    }
    #endregion
}