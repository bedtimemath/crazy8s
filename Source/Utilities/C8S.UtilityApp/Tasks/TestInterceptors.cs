using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;
using SC.Common.Interfaces;

namespace C8S.UtilityApp.Tasks;

internal class TestInterceptors(
    ILogger<TestInterceptors> logger,
    TestInterceptorsOptions options,
    IRandomizer randomizer,
    IDbContextFactory<C8SDbContext> dbContextFactory)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(TestInterceptors)} ===");

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var cnnString = dbContext.Database.GetConnectionString();
        
        Console.WriteLine($"Connection: {cnnString}");
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        // ADD COACH
        var coach = new CoachDb()
        {
            FirstName = String.Empty.AppendRandomAlphaOnly(),
            LastName = String.Empty.AppendRandomAlphaOnly(),
            Email = String.Empty.AppendRandomAlphaOnly() + "@example.com",
            TimeZone = String.Empty.AppendRandomAlphaOnly()
        };
        dbContext.Coaches.Add(coach);
        logger.LogInformation("Added {Coach}", coach.Display);

        // MODIFY 5 APPLICATIONS
        var applications = await dbContext.Applications
            .OrderByDescending(a => a.CreatedOn)
            .Take(5)
            .ToListAsync();
        foreach (var application in applications)
        {
            application.ApplicantFirstName = String.Empty.AppendRandomAlphaOnly(8);
            application.ApplicantLastName = String.Empty.AppendRandomAlphaOnly(8);
            logger.LogInformation("Modified {Application}", application.Display);
        }

        // REMOVE APPLICATION
        var toDelete = await dbContext.Applications
            .OrderBy(a => a.CreatedOn)
            .Skip(randomizer.GetIntBetween(100,1000))
            .FirstAsync();
        logger.LogInformation("Deleting {Application}", toDelete.Display);
        dbContext.Applications.Remove(toDelete);

        // UPDATE THE DATABASE
        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Name}: complete.", nameof(TestInterceptors));
        return 0;
    }
}