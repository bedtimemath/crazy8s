using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Common.Extensions;
using SC.Common.Interactions;

namespace C8S.AdminApp.Controllers;

[Route("api/[controller]/{requestId:int}")]
[ApiController]
public class RequestController(
    ILoggerFactory loggerFactory,
    IDbContextFactory<C8SDbContext> dbContextFactory) : ControllerBase
{
    private readonly ILogger<RequestController> _logger = loggerFactory.CreateLogger<RequestController>();

    [HttpGet]
    public async Task<BackendResponse<RequestDetails>> GetRequest(int requestId)
    {
        try
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var queryable = dbContext.Requests
                .Include(a => a.Place)
                .Include(a => a.RequestedClubs)
                .AsSingleQuery()
                .AsNoTracking();

            var request = await queryable.FirstOrDefaultAsync(r => r.RequestId == requestId);

            return new BackendResponse<RequestDetails>()
            {
                // todo: switch to AutoMapper
                Result = (request == null) ? null :
                    new RequestDetails()
                    {
                        RequestId = request.RequestId,
                        Status = request.Status,
                        PersonLastName = request.PersonLastName,
                        PersonEmail = request.PersonEmail,
                        PersonFirstName = request.PersonFirstName,
                        SubmittedOn = request.SubmittedOn,
                        PlaceName = request.PlaceName,
                        PlaceCity = request.PlaceCity,
                        PlaceState = request.PlaceState
                    }
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while getting details: {Id}", requestId);
            return new BackendResponse<RequestDetails>()
            {
                Exception = exception.ToSerializableException()
            };
        }

    }
}