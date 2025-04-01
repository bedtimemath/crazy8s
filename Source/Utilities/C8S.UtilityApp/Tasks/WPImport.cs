using System.Diagnostics;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.WordPress.Abstractions.Extensions;
using C8S.WordPress.Abstractions.Models;
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

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dbKitPages = await dbContext.KitPages.Include(kp => kp.Kits).ToListAsync();
        _logger.LogInformation("Found {Count:#,##0} kit pages in C8s database.", dbKitPages.Count);

        //foreach (var dbSku in dbSkus)
        //    _logger.LogInformation("{@Sku}", dbSku);

        var wpKitPages = await wordPressService.GetWordPressKitPages();
        _logger.LogInformation("Found {Count:#,##0} kit pages in WordPress.", wpKitPages.Count);

        //foreach (var wpSku in wpSkus)
        //    _logger.LogInformation("{@Sku}", wpSku);

        var wpRoles = await wordPressService.GetWordPressRoles();
        _logger.LogInformation("Found {Count:#,##0} roles in WordPress.", wpRoles.Count);

        var pagesCreated = 0;
        var pagesSkipped = 0;

        var rolesCreated = 0;
        var rolesSkipped = 0;

        ConsoleEx.StartProgress("Adding kit pages to WordPress: ");
        foreach (var dbKitPage in dbKitPages)
        {
            var kit = dbKitPage.Kits.FirstOrDefault() ??
                      throw new UnreachableException("KitPage has no kits.");
            var slug = kit.Key.ToSlug();
            var display = dbKitPage.Title;

            if (wpKitPages.Any(s => s.Properties?.Key == kit.Key))
            {
                pagesSkipped++;
            }
            else
            {
                var wpKitPage = new WPKitPageCreate()
                {
                    Slug = slug,
                    Title = display,
                    Status = "publish", 
                    Properties = new WPKitPageProperties()
                    {
                        Key = kit.Key,
                        Year = kit.Year,
                        Season = kit.Season,
                        AgeLevel = kit.AgeLevel switch {
                            AgeLevel.GradesK2 => "K2",
                            AgeLevel.Grades35 => "35",
                            _ => throw new ArgumentOutOfRangeException()
                        },
                        Version = kit.Version,
                    }
                };

                var details = await wordPressService.CreateWordPressKitPage(wpKitPage);
                pagesCreated++;
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

            ConsoleEx.ShowProgress((float)(pagesCreated + pagesSkipped) / (float)dbKitPages.Count);
        }
        ConsoleEx.EndProgress();

        _logger.LogInformation("{Created:#,##0} skus created, {Skipped:#,##0} skipped.", pagesCreated, pagesSkipped);
        _logger.LogInformation("{Created:#,##0} roles created, {Skipped:#,##0} skipped.", rolesCreated, rolesSkipped); 

        _logger.LogInformation("{Name}: complete.", nameof(WPImport));
        return 0;
    }
}