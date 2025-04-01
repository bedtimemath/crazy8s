using System.Text.Json;
using AutoMapper;
using C8S.AdminApp.Extensions;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Offers.Enums;
using C8S.Domain.Features.Persons.Models;
using C8S.Domain.Features.Persons.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<WrappedListResponse<Person>> GetPersons(
    [FromBody] PersonsListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .AsSingleQuery()
                .AsNoTracking()
                .FilterSortPersonsQueryable(query);
            var total = await queryable.CountAsync();

            queryable = queryable.SkipAndTakePersonsQueryable(query);
            var persons = await mapper.ProjectTo<Person>(queryable).ToListAsync();

            return WrappedListResponse<Person>.CreateSuccessResponse(persons, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<Person>.CreateFailureResponse(exception);
        }
    }

    [HttpPost]
    [Route("api/[controller]/with-orders")]
    public async Task<WrappedListResponse<PersonWithOrders>> GetPersonsWithOrders(
    [FromBody] PersonsWithOrdersListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .Include(p => p.ClubPersons)
                .ThenInclude(cp => cp.Club)
                .ThenInclude(c => c.Orders)
                .ThenInclude(o => o.OrderOffers)
                .ThenInclude(os => os.Offer)
                .AsSingleQuery()
                .AsNoTracking()
                .FilterSortPersonsQueryable(query);

            if (query.SkuYears.Any())
            {
                List<string> skuYears = [];
                if (query.SkuYears.Contains(KitYearOption.PreF21))
                {
                    skuYears.Add("F18");
                    skuYears.Add("F19");
                    skuYears.Add("F19ALT");
                    skuYears.Add("F20");
                }
                if (query.SkuYears.Contains(KitYearOption.F21ToF23))
                {
                    skuYears.Add("F21");
                    skuYears.Add("F22");
                    skuYears.Add("F23");
                    skuYears.Add("F23C");
                }
                if (query.SkuYears.Contains(KitYearOption.F24Plus))
                {
                    skuYears.Add("F24");
                }
                queryable = queryable
                    .Where(p => p.ClubPersons
                        .Any(cp => cp.Club.Orders
                            .Any(o => o.OrderOffers
                                .Any(os => skuYears.Contains(os.Offer.Year)))));
            }

            var total = await queryable.CountAsync();

            queryable = queryable.SkipAndTakePersonsQueryable(query);
            var persons = await mapper.ProjectTo<PersonWithOrders>(queryable).ToListAsync();

            return WrappedListResponse<PersonWithOrders>.CreateSuccessResponse(persons, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<PersonWithOrders>.CreateFailureResponse(exception);
        }

    } 
    #endregion

    #region GET SINGLE
    [HttpGet]
    [Route("api/[controller]/{personId:int}")]
    public async Task<WrappedResponse<Person>> GetPerson(int personId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .AsSingleQuery()
                .AsNoTracking();

            var person = await queryable.FirstOrDefaultAsync(r => r.PersonId == personId);

            return WrappedResponse<Person>.CreateSuccessResponse(mapper.Map<Person?>(person));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", personId);
            return WrappedResponse<Person>.CreateFailureResponse(exception);
        }
    }

    [HttpGet]
    [Route("api/[controller]/with-orders/{personId:int}")]
    public async Task<WrappedResponse<PersonWithOrders>> GetPersonClubOrders(int personId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Persons
                .Include(p => p.ClubPersons)
                .ThenInclude(cp => cp.Club)
                .ThenInclude(c => c.Orders)
                .ThenInclude(o => o.OrderOffers)
                .ThenInclude(os => os.Offer)
                .AsSingleQuery()
                .AsNoTracking();

            var person = await queryable.FirstOrDefaultAsync(r => r.PersonId == personId);

            return WrappedResponse<PersonWithOrders>.CreateSuccessResponse(mapper.Map<PersonWithOrders?>(person)); 
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", personId);
            return WrappedResponse<PersonWithOrders>.CreateFailureResponse(exception);
        }
    }
    #endregion
}