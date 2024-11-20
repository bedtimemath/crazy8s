using System.Net.Http.Json;
using C8S.Domain.AppConfigs;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Models;
using SC.Common.Extensions;
using SC.Common.Interfaces;

namespace C8S.UtilityApp.Tasks;

internal class AddApplication(
    ILogger<AddApplication> logger,
    AddApplicationOptions options,
    IRandomizer randomizer,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IHttpClientFactory httpClientFactory)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(AddApplication)} ===");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cnnString = dbContext.Database.GetConnectionString();
        
        Console.WriteLine($"Connection: {cnnString}");
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        // ADD APPLICATION
        var application = new ApplicationDb()
        {
            Status = ApplicationStatus.Received,
            ApplicantType = (randomizer.GetDouble() <= 0.5) ? ApplicantType.Coach : ApplicantType.Supervisor,
            ApplicantFirstName = String.Empty.AppendRandomAlphaOnly(),
            ApplicantLastName = String.Empty.AppendRandomAlphaOnly(),
            ApplicantEmail = String.Empty.AppendRandomAlphaOnly() + "@example.com",
            ApplicantTimeZone = String.Empty.AppendRandomAlphaOnly(),
            SubmittedOn = DateTimeOffset.UtcNow
        };
        var applicationClubs = Enumerable.Range(0, 3)
            .Select(_ => new ApplicationClubDb()
            {
                Application = application,
                AgeLevel = (AgeLevel)(randomizer.GetIntLessThan(2)),
                ClubSize = ClubSize.Size16,
                Season = randomizer.GetIntBetween(1,3),
                StartsOn = DateOnly.FromDateTime(DateTime.Today.AddDays(randomizer.GetIntBetween(21,50)))
            })
            .ToList();
        application.ApplicationClubs = applicationClubs;
        dbContext.Applications.Add(application);

        // UPDATE THE DATABASE
        await dbContext.SaveChangesAsync();
        logger.LogInformation("Added {Application}", application.Display);

        // ALERT THROUGH THE ENDPOINT
        var httpClient = httpClientFactory.CreateClient(nameof(Endpoints.C8SAdminApp));
        var dataChange = new DataChange()
        {
            EntityId = application.ApplicationId,
            EntityName = nameof(ApplicationDb),
            EntityState = EntityState.Added
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

        logger.LogInformation("{Name}: complete.", nameof(AddApplication));
        return 0;
    }
}