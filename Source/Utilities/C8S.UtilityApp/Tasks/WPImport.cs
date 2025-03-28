﻿using C8S.Domain.EFCore.Contexts;
using C8S.UtilityApp.Base;
using C8S.WordPress.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

#if false
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dbSkus = await dbContext.Offers
            .Where(s => (s.Year == "F23" || s.Year == "F23C" || s.Year == "F24") &&
                        (s.Title != "dont use"))
            .ToListAsync();
        _logger.LogInformation("Found {Count:#,##0} active skus in C8s database.", dbSkus.Count);

        //foreach (var dbSku in dbSkus)
        //    _logger.LogInformation("{@Sku}", dbSku);

        var wpSkus = await wordPressService.GetWordPressSkus();
        _logger.LogInformation("Found {Count:#,##0} skus in WordPress.", wpSkus.Count);

        //foreach (var wpSku in wpSkus)
        //    _logger.LogInformation("{@Sku}", wpSku);

        var wpRoles = await wordPressService.GetWordPressRoles();
        _logger.LogInformation("Found {Count:#,##0} roles in WordPress.", wpRoles.Count);

        var skusCreated = 0;
        var skusSkipped = 0;

        var rolesCreated = 0;
        var rolesSkipped = 0;

        ConsoleEx.StartProgress("Adding skus to WordPress: ");
        foreach (var dbSku in dbSkus)
        {
            var slug = dbSku.ClubKey.ToSlug();
            var display = dbSku.Title;

            if (wpSkus.Any(s => s.Properties.SkuIdentifier == dbSku.ClubKey))
            {
                skusSkipped++;
            }
            else
            {
                var wpSku = new WPSkuCreate()
                {
                    Slug = slug,
                    Title = display,
                    Status = OfferStatus.Active,
                    Properties = new WPSkuProperties()
                    {
                        SkuIdentifier = dbSku.ClubKey,
                        Season = dbSku.Season,
                        AgeLevel = dbSku.AgeLevel
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
#endif

        _logger.LogInformation("{Name}: complete.", nameof(WPImport));
        return 0;
    }
}