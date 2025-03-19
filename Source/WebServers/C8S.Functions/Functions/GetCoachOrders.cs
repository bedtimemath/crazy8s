using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Features.Persons.Models;
using C8S.Functions.DTOs;
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
                var idString = req.Query["id"];
                if (String.IsNullOrEmpty(idString))
                    throw new ArgumentNullException("id");
                if (!Int32.TryParse(idString, out var wordPressId))
                    throw new ArgumentNullException("id");

                // now look for the matching person
                await using var dbContext = await dbContextFactory.CreateDbContextAsync();

                var personDTO = (PersonDTO?)null;
                var clubDTOs = (List<ClubDTO>?)null;

                var personDb = await dbContext.Persons
                    .AsNoTracking()
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(p => p.WordPressId == wordPressId);

                // no point in checking clubs if we can't find the person
                if (personDb != null)
                {
                    personDTO = mapper.Map<PersonDTO>(personDb);
                    var clubDbs = await dbContext.Clubs
                        .Include(c => c.Orders)
                        .ThenInclude(o => o.Shipments)
                        .Include(c => c.Orders)
                        .ThenInclude(o => o.OrderSkus)
                        .ThenInclude(os => os.Sku)
                        .Where(c => c.ClubPersons.Any(cp => cp.Person.WordPressId == wordPressId))
                        .AsNoTracking()
                        .AsSingleQuery()
                        .ToListAsync();
                    clubDTOs = clubDbs.Select(mapper.Map<ClubDTO>).ToList();
                } 

#if false
                _logger.LogDebug("Clubs: {@Clubs}", clubDTOs);
                var personDTO = idString;
                List<string> clubDTOs = ["one", "two", "three"];
#endif

                // send back the person with their clubs/orders
                response = await req.CreateSuccessResponse(new PersonClubs()
                {
                    Person = personDTO,
                    Clubs = clubDTOs
                });
            }
            catch (Exception ex)
            {
                response = await req.CreateFailureResponse(ex);
            }

            return response;
        }
    }
}

public record PersonClubs
{
    [JsonPropertyName("person")]
    public PersonDTO? Person { get; init; } = null!;

    [JsonPropertyName("clubs")]
    public List<ClubDTO>? Clubs { get; init; } = null!;
}