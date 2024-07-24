using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Contexts;
using C8S.Database.Repository.Repositories;
using C8S.UtilityApp.Models;
using C8S.UtilityApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class LoadC8SData(
    ILogger<LoadC8SData> logger,
    LoadC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IMapper mapper,
    OldSystemService oldSystemService,
    C8SRepository repository)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        logger.LogInformation("=== {Name} ===", nameof(LoadC8SData));

        if (!Directory.Exists(options.InputPath))
            throw new Exception($"Missing input path: {options.InputPath}");

        // check the connection quickly
        // ReSharper disable once UseAwaitUsing
        // ReSharper disable once MethodHasAsyncOverload
        using (var tempDb = dbContextFactory.CreateDbContext())
        {
            var sqlConnection = tempDb.Database.GetConnectionString();
            logger.LogInformation("Connection: {Connection}", sqlConnection);
        }
        logger.LogInformation("OldSystem: {Connection}", oldSystemService.ConnectionString);

        Console.Write($"");

        logger.LogInformation("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        var organizations = (await oldSystemService.GetOrganizations())
            .Select(mapper.Map<OrganizationSql, OrganizationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} organizations", organizations.Count);

        var addeds = await repository.AddOrganizations(organizations);
        logger.LogInformation("Added {Count:#,##0} organizations", addeds.Count());

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}