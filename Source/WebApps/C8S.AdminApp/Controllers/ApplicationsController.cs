using System.Linq.Dynamic.Core;
using System.Text.Json;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Models;
using C8S.Domain.Queries;
using C8S.Domain.Queries.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationsController(
    ILoggerFactory loggerFactory,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<ApplicationsController> _logger = loggerFactory.CreateLogger<ApplicationsController>();

    [HttpPost]
    public async Task<BackendResponse<ApplicationListResults>> GetApplications(
        [FromBody] ListApplicationsQuery query)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Requests
                .Include(a => a.Place)
                .Include(a => a.RequestedClubs)
                .AsSingleQuery()
                .AsNoTracking();

            if (!String.IsNullOrEmpty(query.SortDescription)) 
                queryable = queryable.OrderBy(query.SortDescription);
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
            
            var totalApplications = await queryable.CountAsync();

            if (query.StartIndex != null) queryable = queryable.Skip(query.StartIndex.Value);
            if (query.Count != null) queryable = queryable.Take(query.Count.Value);

            var applications = await queryable.ToListAsync();

            return new BackendResponse<ApplicationListResults>()
            {
                Result = new ApplicationListResults()
                {
                    Items = applications
                        .Select(a => new ApplicationListDisplay()
                        {
                            ApplicationId = a.RequestId,
                            ApplicantFirstName = a.PersonFirstName,
                            ApplicantLastName = a.PersonLastName,
                            ApplicantEmail = a.PersonEmail,
                            Status = a.Status,
                            SubmittedOn = a.SubmittedOn,
                            OrganizationName = a.PlaceName
                        })
                        .ToList(),
                    Total = totalApplications
                }
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Error while executing query: {JsonSerializer.Serialize(query)}");
            return new BackendResponse<ApplicationListResults>()
            {
                Exception = exception.ToSerializableException()
            };
        }

    }
}