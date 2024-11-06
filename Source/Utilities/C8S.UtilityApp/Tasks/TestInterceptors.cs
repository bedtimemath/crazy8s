using C8S.Database.EFCore.Contexts;
using C8S.UtilityApp.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        var application = await dbContext.Applications
            .OrderByDescending(a => a.CreatedOn)
            .FirstOrDefaultAsync() ??
                          throw new Exception("No applications found.");

        logger.LogInformation("Application: {@App}", application);

        application.ModifiedOn = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Application: {@App}", application);

        logger.LogInformation("{Name}: complete.", nameof(TestInterceptors));
        return 0;
    }
}