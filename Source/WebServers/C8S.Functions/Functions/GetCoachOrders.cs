using System.Net;
using System.Text.Json;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Persons.Models;
using C8S.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Functions.Functions
{
    public class GetCoachOrders(
        ILoggerFactory loggerFactory,
        IDbContextFactory<C8SDbContext> dbContextFactory,
        IMapper mapper)
    {
        #region ReadOnly Constructor Variables

        private readonly ILogger<GetCoachOrders> _logger = loggerFactory.CreateLogger<GetCoachOrders>();
        #endregion

        [Function("GetCoachOrders")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "coach")] HttpRequestData req)
        {
            HttpResponseData response;
            try
            {
                _logger.LogInformation("GetCoachOrders triggered");

                // start by checking that we got a proper id value
                // start by checking that we got a proper id value
                var idString = req.Query["id"];
                if (String.IsNullOrEmpty(idString))
                    throw new ArgumentNullException("id");
                if (!Int32.TryParse(idString, out var wordPressId))
                    throw new ArgumentNullException("id");

                // now look for the matching person
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();
                var personDb = await dbContext.Persons
                    .Include(p => p.ClubPersons)
                    .ThenInclude(cp => cp.Club)
                    .ThenInclude(c => c.Orders)
                    .ThenInclude(o => o.OrderSkus)
                    .AsNoTracking()
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(p => p.WordPressId == wordPressId);
                if (personDb == null)
                    throw new ArgumentOutOfRangeException("id");

                // send back the person with their orders
                response = await req.CreateSuccessResponse(mapper.Map<PersonWithOrders>(personDb));
            }
            catch (Exception ex)
            {
                response = await req.CreateFailureResponse(ex);
            }

            return response;
        }
    }
}
