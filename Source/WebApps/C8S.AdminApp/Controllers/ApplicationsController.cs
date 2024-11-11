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
            var queryable = dbContext.Applications
                .Include(a => a.LinkedOrganization)
                .Include(a => a.Address)
                .Include(a => a.ApplicationClubs)
                .Where(a => !String.IsNullOrWhiteSpace(a.ApplicantLastName) &&
                            !String.IsNullOrWhiteSpace(a.ApplicantEmail))
                .AsSingleQuery()
                .AsNoTracking();

            if (!String.IsNullOrEmpty(query.SortDescription)) queryable = queryable.OrderBy(query.SortDescription);
            
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
                            ApplicationId = a.ApplicationId,
                            ApplicantFirstName = a.ApplicantFirstName,
                            ApplicantLastName = a.ApplicantLastName,
                            ApplicantEmail = a.ApplicantEmail,
                            Status = a.Status,
                            SubmittedOn = a.SubmittedOn,
                            OrganizationName = a.OrganizationName,
                            OrganizationCity = a.Address?.City,
                            OrganizationState = a.Address?.State
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