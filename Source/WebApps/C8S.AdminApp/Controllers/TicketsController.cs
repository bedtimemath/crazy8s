using System.Linq.Dynamic.Core;
using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Tickets.Models;
using C8S.Domain.Features.Tickets.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Responses;

namespace C8S.AdminApp.Controllers;

[ApiController]
public class TicketsController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<TicketsController> _logger = loggerFactory.CreateLogger<TicketsController>();

    #region GET LIST
    [HttpPost]
    [Route("api/[controller]")]
    public async Task<WrappedListResponse<TicketListItem>> GetTickets(
    [FromBody] TicketsListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Tickets
                .Include(t => t.Request)
                .Include(t => t.Place)
                .Include(t => t.TicketPersons)
                .ThenInclude(tp => tp.Person)
                .AsSingleQuery()
                .AsNoTracking();

            /*** QUERY ***/
            if (!String.IsNullOrEmpty(query.Query))
            {
                //queryable = queryable.Where(r => (!String.IsNullOrEmpty(r.PersonFirstName) && r.PersonFirstName.Contains(query.Query)) ||
                //                                 r.PersonLastName.Contains(query.Query) ||
                //                                 r.PersonEmail.Contains(query.Query));
            }

            /*** HAS COACH CALL ***/
            if (query.HasCoachCall != null)
            {
                queryable = queryable.Where(t =>
                    ((t.Request == null || t.Request.AppointmentStartsOn == null) ^ query.HasCoachCall.Value));
            }

            /*** SUBMITTED BEFORE / AFTER ***/
            if (query.SubmittedAfter != null)
            {
                queryable = queryable.Where(t => t.Request != null &&
                                                 t.Request.SubmittedOn >= query.SubmittedAfter.Value.ToDateTime(TimeOnly.MinValue));
            }
            if (query.SubmittedBefore != null)
            {
                queryable = queryable.Where(t => t.Request != null &&
                                                 t.Request.SubmittedOn <= query.SubmittedBefore.Value.ToDateTime(TimeOnly.MinValue));
            }

            /*** STATUS ***/
            if (query.Statuses != null && query.Statuses.Any())
            {
                var statusClauses = new List<string>();
                var paramObjects = new object?[query.Statuses.Count];
                for (int index = 0; index < query.Statuses.Count; index++)
                {
                    statusClauses.Add($"x.Status == @{index}");
                    paramObjects[index] = query.Statuses[index];
                }
                var dynamicWhere = "x => " + String.Join(" || ", statusClauses);
                _logger.LogInformation("WHERE: {Where}", dynamicWhere);
                queryable = queryable.Where(dynamicWhere, paramObjects);
            }
            
            var total = await queryable.CountAsync();

            if (query.StartIndex != null) queryable = queryable.Skip(query.StartIndex.Value);
            if (query.Count != null) queryable = queryable.Take(query.Count.Value);

            var tickets = await mapper.ProjectTo<TicketListItem>(queryable).ToListAsync();

            return WrappedListResponse<TicketListItem>.CreateSuccessResponse(tickets, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<TicketListItem>.CreateFailureResponse(exception);
        }
    }

#if false
    [HttpPost]
    [Route("api/[controller]/with-orders")]
    public async Task<WrappedListResponse<TicketWithOrders>> GetTicketsWithOrders(
[FromBody] TicketsWithOrdersListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Tickets
                .Include(p => p.ClubTickets)
                .ThenInclude(cp => cp.Club)
                .ThenInclude(c => c.Orders)
                .ThenInclude(o => o.OrderOffers)
                .ThenInclude(os => os.Offer)
                .AsSingleQuery()
                .AsNoTracking()
                .FilterSortTicketsQueryable(query);

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
                    .Where(p => p.ClubTickets
                        .Any(cp => cp.Club.Orders
                            .Any(o => o.OrderOffers
                                .Any(os => skuYears.Contains(os.Offer.Year)))));
            }

            var total = await queryable.CountAsync();

            queryable = queryable.SkipAndTakeTicketsQueryable(query);
            var tickets = await mapper.ProjectTo<TicketWithOrders>(queryable).ToListAsync();

            return WrappedListResponse<TicketWithOrders>.CreateSuccessResponse(tickets, total);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return WrappedListResponse<TicketWithOrders>.CreateFailureResponse(exception);
        }

    }  
#endif
    #endregion

    #region GET SINGLE
#if false
    [HttpGet]
    [Route("api/[controller]/{ticketId:int}")]
    public async Task<WrappedResponse<Ticket>> GetTicket(int ticketId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Tickets
                .AsSingleQuery()
                .AsNoTracking();

            var ticket = await queryable.FirstOrDefaultAsync(r => r.TicketId == ticketId);

            return WrappedResponse<Ticket>.CreateSuccessResponse(mapper.Map<Ticket?>(ticket));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", ticketId);
            return WrappedResponse<Ticket>.CreateFailureResponse(exception);
        }
    }

    [HttpGet]
    [Route("api/[controller]/with-orders/{ticketId:int}")]
    public async Task<WrappedResponse<TicketWithOrders>> GetTicketClubOrders(int ticketId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Tickets
                .Include(p => p.ClubTickets)
                .ThenInclude(cp => cp.Club)
                .ThenInclude(c => c.Orders)
                .ThenInclude(o => o.OrderOffers)
                .ThenInclude(os => os.Offer)
                .AsSingleQuery()
                .AsNoTracking();

            var ticket = await queryable.FirstOrDefaultAsync(r => r.TicketId == ticketId);

            return WrappedResponse<TicketWithOrders>.CreateSuccessResponse(mapper.Map<TicketWithOrders?>(ticket));
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", ticketId);
            return WrappedResponse<TicketWithOrders>.CreateFailureResponse(exception);
        }
    } 
#endif
    #endregion
}