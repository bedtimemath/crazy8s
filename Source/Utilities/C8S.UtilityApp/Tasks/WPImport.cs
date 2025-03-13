using System.Diagnostics;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.WordPress.Abstractions.Models;
using C8S.WordPress.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common.Extensions;

namespace C8S.UtilityApp.Tasks;

internal class WPImport(
    ILoggerFactory loggerFactory,
    WordPressService wordPressService,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    WPImportOptions options)
    : IActionLauncher
{
    private readonly ILogger<WPImport> _logger = loggerFactory.CreateLogger<WPImport>();

    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(WPImport)} ===");
        Console.WriteLine($"Site: {options.Site}");

        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dbSkus = await dbContext.Skus
            .Where(s => (s.Year == "F23" || s.Year == "F23C" || s.Year == "F24") && 
                        (s.Name != "dont use"))
            .ToListAsync();
        _logger.LogInformation("Found {Count:#,##0} active skus in C8s database.", dbSkus.Count);

        //foreach (var dbSku in dbSkus)
        //    _logger.LogInformation("{@Sku}", dbSku);

        var skusResponse = await wordPressService.GetWordPressSkus(); 
        var wpSkus = skusResponse.Result ?? [];
        _logger.LogInformation("Found {Count:#,##0} skus in WordPress.", skusResponse.Total);

        //foreach (var wpSku in wpSkus)
        //    _logger.LogInformation("{@Sku}", wpSku);

        var rolesResponse = await wordPressService.GetWordPressRoles(); 
        var wpRoles = rolesResponse.Result ?? [];
        _logger.LogInformation("Found {Count:#,##0} roles in WordPress.", rolesResponse.Total);

        var skusCreated = 0;
        var skusSkipped = 0;

        var rolesCreated = 0;
        var rolesSkipped = 0;
        
        ConsoleEx.StartProgress("Adding skus to WordPress: ");
        foreach (var dbSku in dbSkus)
        {
            var slug = dbSku.Key.RemoveNonAlphanumeric("_").ToLowerInvariant();
            var display = dbSku.Name;

            if (wpSkus.Any(s => s.Properties.SkuIdentifier == dbSku.Key))
            {
                skusSkipped++;
            }
            else
            {
                var wpSku = new WPSkuCreate()
                {
                    Slug = slug,
                    Title = display,
                    Status = SkuStatus.Active,
                    Properties = new WPSkuProperties()
                    {
                        SkuIdentifier = dbSku.Key,
                        Season = dbSku.Season ?? 1,
                        AgeLevel = dbSku.AgeLevel ?? AgeLevel.GradesK2
                    }
                };

                await wordPressService.CreateWordPressSku(wpSku);
                skusCreated++;
            }

            if (wpRoles.Any(r => r.Slug == slug))
            {
                rolesSkipped++;
            }
            else
            {
                var wpRole = new WPRoleDetails()
                {
                    Slug = slug,
                    Display = display,
                    Capabilities = [$"can_view_{slug}"]
                };

                var created = await wordPressService.CreateWordPressRole(wpRole);
                if (created.Slug != slug)
                    throw new UnreachableException($"Created role doesn't match request: {created.Slug}");
                rolesCreated++;
            }

            ConsoleEx.ShowProgress((float)(skusCreated + skusSkipped) / (float)dbSkus.Count);
        }
        ConsoleEx.EndProgress();

        _logger.LogInformation("{Created:#,##0} skus created, {Skipped:#,##0} skipped.", skusCreated, skusSkipped);
        _logger.LogInformation("{Created:#,##0} roles created, {Skipped:#,##0} skipped.", rolesCreated, rolesSkipped);

        _logger.LogInformation("{Name}: complete.", nameof(WPImport));
        return 0;
    }
}