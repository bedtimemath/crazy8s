using System.Diagnostics;
using C8S.Domain.EFCore.Contexts;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using C8S.UtilityApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class FixC8SData(
    ILogger<FixC8SData> logger,
    FixC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    OldSystemService oldSystemService)
    : BaseC8SData(dbContextFactory, oldSystemService)
{
    public override async Task<int> Launch()
    {
        logger.LogInformation("=== {Name} ===", nameof(FixC8SData));

        if (!Directory.Exists(options.InputPath))
            throw new Exception($"Missing input path: {options.InputPath}");

        // check the connection quickly
        // ReSharper disable once UseAwaitUsing
        // ReSharper disable once MethodHasAsyncOverload
        using (var tempDb = DbContextFactory.CreateDbContext())
        {
            var sqlConnection = tempDb.Database.GetConnectionString();
            logger.LogInformation("Connection: {Connection}", sqlConnection);
        }
        logger.LogInformation("OldSystem: {Connection}", OldSystemService.ConnectionString);

        Console.Write($"");

        logger.LogInformation("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;
        Console.WriteLine();

        var sqlApplications = await OldSystemService.GetApplications();
        var ticketLookup = await CreateTicketsLookup();
        var (updated, skipped) = await AddScheduledCalls(sqlApplications, ticketLookup);
        logger.LogInformation("Updated {Count:#,##0} applications with scheduled calls; {Skipped:#,##0} skipped.", updated, skipped);

        logger.LogInformation("{Name}: complete.", nameof(FixC8SData));
        return 0;
    }

    private async Task<(int,int)> AddScheduledCalls(List<ApplicationSql> sqlApplications,
        Dictionary<Guid,int> ticketLookup)
    {
        var updated = 0;
        var skipped = 0;

        var appsToCheck = sqlApplications.Where(a => a.ScheduledCall != null).ToList();
        ConsoleEx.StartProgress($"Checking {appsToCheck.Count:#,##0} for missing calls: ");
        foreach (var app in appsToCheck)
        {
            await using var dbContext = await DbContextFactory.CreateDbContextAsync();

            var ticketId = ticketLookup.GetValueOrDefault(app.OldSystemApplicationId!.Value);
            if (ticketId == 0) 
                throw new UnreachableException($"Ticket not found in lookup: Guid# {app.OldSystemApplicationId}");
            
            var ticket = await dbContext.Tickets
                .Include(t => t.Request)
                .AsSingleQuery()
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
            if (ticket == null)
                throw new UnreachableException($"Could not find ticket: ID# {ticketId}");
            if (ticket.Request == null)
                throw new UnreachableException($"Ticket does not have request: ID# {ticketId}");

            if (ticket.Request.AppointmentStartsOn != null)
                skipped++;

            else
            {
                ticket.Request.AppointmentStartsOn = app.ScheduledCall;
                updated++;
                await dbContext.SaveChangesAsync();
            }

            ConsoleEx.ShowProgress((float)(updated+skipped) / (float)appsToCheck.Count);
        }
        ConsoleEx.EndProgress();

        return (updated, skipped);
    }
}