using C8S.Database.EFCore.Contexts;
using C8S.Database.EFCore.Models;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.UtilityApp.Tasks;

internal class TestInterceptors(
    ILogger<TestInterceptors> logger,
    TestInterceptorsOptions options,
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

        //var coach = new CoachDb()
        //{
        //    FirstName = String.Empty.AppendRandomAlphaOnly(),
        //    LastName = String.Empty.AppendRandomAlphaOnly(),
        //    Email = String.Empty.AppendRandomAlphaOnly() + "@example.com",
        //    TimeZone = String.Empty.AppendRandomAlphaOnly()
        //};
        //dbContext.Coaches.Add(coach);

        var application = await dbContext.Applications
            .OrderByDescending(a => a.CreatedOn)
            .FirstOrDefaultAsync() ??
                          throw new Exception("No applications found.");
        application.ApplicantFirstName = String.Empty.AppendRandomAlphaOnly(8);
        application.ApplicantLastName = String.Empty.AppendRandomAlphaOnly(8);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Name}: complete.", nameof(TestInterceptors));
        return 0;
    }
}