using System.Text.Json;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Models;
using C8S.Domain.Queries;
using C8S.Domain.Queries.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
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
            var totalApplications = await dbContext.Applications.CountAsync();
            var queryable = dbContext.Applications
                .AsNoTracking();

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
                            Status = a.Status,
                            SubmittedOn = a.SubmittedOn
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