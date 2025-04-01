using System.Text.Json.Serialization;
using AutoMapper;
using C8S.Domain.EFCore.Contexts;
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
                        .ThenInclude(o => o.OrderOffers)
                        .ThenInclude(os => os.Offer)
                        .Where(c => c.ClubPersons.Any(cp => cp.Person.WordPressId == wordPressId))
                        .AsNoTracking()
                        .AsSingleQuery()
                        .ToListAsync();
                    clubDTOs = clubDbs.Select(mapper.Map<ClubDTO>).ToList();
                }

                // send back the person with their clubs/orders
                var personClubs = personDTO == null ? null : new PersonClubs() { Person = personDTO, Clubs = clubDTOs };
                response = await req.CreateSuccessResponse(personClubs);
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