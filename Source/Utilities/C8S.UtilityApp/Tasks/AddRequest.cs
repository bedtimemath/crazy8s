using System.Net.Http.Json;
using C8S.Domain.AppConfigs;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Requests.Enums;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using SC.Common.Interfaces;
using SC.Common.PubSub;

namespace C8S.UtilityApp.Tasks;

internal class AddRequest(
    ILogger<AddRequest> logger,
    IRandomizer randomizer,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IHttpClientFactory httpClientFactory)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(AddRequest)} ===");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cnnString = dbContext.Database.GetConnectionString();
        
        Console.WriteLine($"Connection: {cnnString}");
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        // ADD REQUEST
        var request = new RequestDb()
        {
            Status = RequestStatus.Received,
            PersonType = (randomizer.GetDouble() <= 0.5) ? ApplicantType.Coach : ApplicantType.Supervisor,
            PersonFirstName = String.Empty.AppendRandomAlphaOnly(),
            PersonLastName = String.Empty.AppendRandomAlphaOnly(),
            PersonEmail = String.Empty.AppendRandomAlphaOnly() + "@example.com",
            PersonTimeZone = String.Empty.AppendRandomAlphaOnly(),
            SubmittedOn = DateTimeOffset.UtcNow
        };
        var requestedClubs = Enumerable.Range(0, 3)
            .Select(_ => new RequestedClubDb()
            {
                Request = request,
                AgeLevel = (AgeLevel)(randomizer.GetIntLessThan(2)),
                Season = randomizer.GetIntBetween(1,3),
                StartsOn = DateOnly.FromDateTime(DateTime.Today.AddDays(randomizer.GetIntBetween(21,50)))
            })
            .ToList();
        request.RequestedClubs = requestedClubs;
        dbContext.Requests.Add(request);

        // UPDATE THE DATABASE
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Added {Application}", request.Display);

        // ALERT THROUGH THE ENDPOINT
        var httpClient = httpClientFactory.CreateClient(nameof(Endpoints.C8SAdminApp));
        var dataChange = new DataChange()
        {
            EntityId = request.RequestId,
            EntityName = nameof(RequestDb),
            Action = DataChangeAction.Added
        };

        try
        {
            var response = await httpClient.PostAsJsonAsync("api/datachanges", dataChange);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Bad response status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not post to endpoint");
        }

        logger.LogInformation("{Name}: complete.", nameof(AddRequest));
        return 0;
    }
}