using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.WordPress.Abstractions;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.UtilityApp.Tasks;

internal class WPImportSkus(
    ILoggerFactory loggerFactory,
    WordPressService wordPressService,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    WPImportSkusOptions options)
    : IActionLauncher
{
    private readonly ILogger<WPImportSkus> _logger = loggerFactory.CreateLogger<WPImportSkus>();

    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(WPImportSkus)} ===");
        Console.WriteLine($"Site: {options.Site}");

        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dbSkus = await dbContext.Skus
            .Where(s => s.Status == SkuStatus.Active)
            .ToListAsync();
        _logger.LogInformation("Found {Count:#,##0} active skus in C8s database.", dbSkus.Count);

        //foreach (var dbSku in dbSkus)
        //    _logger.LogInformation("{@Sku}", dbSku);

        var wpSkus = (await wordPressService.GetWordPressSkus()).Items;
        _logger.LogInformation("Found {Count:#,##0} skus in WordPress.", wpSkus.Count);

        //foreach (var wpSku in wpSkus)
        //    _logger.LogInformation("{@Sku}", wpSku);

        var createdCount = 0;
        var skippedCount = 0;

        ConsoleEx.StartProgress("Adding skus to WordPress: ");
        foreach (var dbSku in dbSkus)
        {
            if (wpSkus.Any(s => s.Properties.SkuIdentifier == dbSku.Key))
            {
                skippedCount++;
            }
            else
            {
                var wpSku = new WPSkuCreate()
                {
                    Slug = dbSku.Key.RemoveNonAlphanumeric("_"),
                    Title = dbSku.Name,
                    Status = SkuStatus.Active,
                    Properties = new WPSkuProperties()
                    {
                        SkuIdentifier = dbSku.Key,
                        Season = dbSku.Season ?? 1,
                        AgeLevel = dbSku.AgeLevel ?? AgeLevel.GradesK2
                    }
                };

                await wordPressService.CreateWordPressSku(wpSku);
                createdCount++;
            }
            ConsoleEx.ShowProgress((float)(createdCount + skippedCount) / (float)dbSkus.Count);
        }
        ConsoleEx.EndProgress();

        _logger.LogInformation("{Created:#,##0} skus created, {Skipped:#,##0} skipped.", createdCount, skippedCount);


        _logger.LogInformation("{Name}: complete.", nameof(WPImportSkus));
        return 0;
    }
}