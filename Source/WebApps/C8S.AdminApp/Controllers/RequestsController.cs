using System.Linq.Dynamic.Core;
using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Requests.Models;
using C8S.Domain.Features.Requests.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RequestsController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<RequestsController> _logger = loggerFactory.CreateLogger<RequestsController>();

    [HttpPost]
    public async Task<BackendResponse<RequestListResults>> GetRequests(
        [FromBody] RequestsListQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Requests
                .Include(a => a.Place)
                .Include(a => a.RequestedClubs)
                .AsSingleQuery()
                .AsNoTracking();

            /*** QUERY ***/
            if (!String.IsNullOrEmpty(query.Query))
            {
                queryable = queryable.Where(r => (!String.IsNullOrEmpty(r.PersonFirstName) && r.PersonFirstName.Contains(query.Query)) ||
                                                 r.PersonLastName.Contains(query.Query) ||
                                                 r.PersonEmail.Contains(query.Query));
            }

            /*** HAS COACH CALL ***/
            if (query.HasCoachCall != null)
            {
                queryable = queryable.Where(r => 
                    (r.FullSlateAppointmentStartsOn == null ^ query.HasCoachCall.Value));
            }

            /*** SINCE WHEN ***/
            if (query.SinceWhen != null)
            {
                queryable = queryable.Where(r =>
                    r.SubmittedOn >= query.SinceWhen.Value.ToDateTime(TimeOnly.MinValue));
            }

            /*** SORT ***/
            if (!String.IsNullOrEmpty(query.SortDescription)) 
                queryable = queryable.OrderBy(query.SortDescription);

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
            
            var totalRequests = await queryable.CountAsync();

            if (query.StartIndex != null) queryable = queryable.Skip(query.StartIndex.Value);
            if (query.Count != null) queryable = queryable.Take(query.Count.Value);

            var requests = await queryable.ToListAsync();

            return new BackendResponse<RequestListResults>()
            {
                Result = new RequestListResults()
                {
                    Items = requests
                        .Select(mapper.Map<RequestListItem>)
                        .ToList(),
                    Total = totalRequests
                }
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while executing query: {Query}", JsonSerializer.Serialize(query));
            return new BackendResponse<RequestListResults>()
            {
                Exception = exception.ToSerializableException()
            };
        }

    }
}