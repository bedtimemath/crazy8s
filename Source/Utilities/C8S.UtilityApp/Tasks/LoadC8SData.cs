using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Contexts;
using C8S.Database.Repository.Repositories;
using C8S.UtilityApp.Extensions;
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
        Console.WriteLine();

#if false
        /*** ORGANIZATIONS ***/
        var orgDTOs = (await oldSystemService.GetOrganizations())
            .Select(mapper.Map<OrganizationSql, OrganizationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} organizations", orgDTOs.Count);

        var existingOrgIds = (await repository.GetOrganizations()).Select(o => o.OldSystemOrganizationId).ToList();
        orgDTOs.RemoveAll(m => existingOrgIds.Contains(m.OldSystemOrganizationId));

        var addedOrgs = await repository.AddOrganizations(orgDTOs);
        logger.LogInformation("Added {Count:#,##0} organizations", addedOrgs.Count());

        /*** COACHES ***/
        var coachDTOs = (await oldSystemService.GetCoaches())
            .Select(mapper.Map<CoachSql, CoachDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} coaches", coachDTOs.Count);

        var hasOrgIds = coachDTOs.Where(c => c.OldSystemOrganizationId.HasValue).ToList();
        logger.LogInformation("Found {Count:#,##0} coaches with org ids", hasOrgIds.Count);

        var existingCoachIds = (await repository.GetCoaches()).Select(o => o.OldSystemCoachId).ToList();
        coachDTOs.RemoveAll(m => existingCoachIds.Contains(m.OldSystemCoachId));

        var addedCoaches = await repository.AddCoaches(coachDTOs);
        logger.LogInformation("Added {Count:#,##0} coaches", addedCoaches.Count());

        /*** JOINING COACHES & ORGANIZATIONS ***/
        var allOrganizations = (await repository.GetOrganizations()).ToList();
        var allCoaches = (await repository.GetCoaches()).ToList();
        var totalCoaches = allCoaches.Count;

        var coachesWithOrganization = 0;

        ConsoleEx.StartProgress("Joining coaches with organizations: ");
        for (int index = 0; index < totalCoaches; index++)
        {
            var coach = allCoaches[index];
            if (coach.OldSystemOrganizationId.HasValue)
            {
                var organization = allOrganizations.FirstOrDefault(
                                       o => o.OldSystemOrganizationId == coach.OldSystemOrganizationId.Value) ??
                                   throw new Exception($"Could not find organization: {coach.OldSystemOrganizationId.Value}");
                if (coach.OrganizationId == null)
                {
                    coach.OrganizationId = organization.OrganizationId;

                    await repository.UpdateCoach(coach);
                    coachesWithOrganization++;
                }
            }

            ConsoleEx.ShowProgress((float)index / (float)totalCoaches);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} coaches updated.", coachesWithOrganization);

#endif

        /*** APPLICATIONS ***/
        var applicationDTOs = (await oldSystemService.GetApplications())
           .Select(mapper.Map<ApplicationSql, ApplicationDTO>)
           .ToList();

        logger.LogInformation("Found {Count:#,##0} applications", applicationDTOs.Count);

        /*** APPLICATION CLUBS ***/
        var applicationClubDTOs = (await oldSystemService.GetApplicationClubs())
           .Select(mapper.Map<ApplicationClubSql, ApplicationClubDTO>)
           .ToList();

        logger.LogInformation("Found {Count:#,##0} application clubs", applicationClubDTOs.Count);

#if false
        var hasOrgIds = applicationDTOs.Where(c => c.OldSystemOrganizationId.HasValue).ToList();
        logger.LogInformation("Found {Count:#,##0} applications with org ids", hasOrgIds.Count);

        var existingApplicationIds = (await repository.GetApplications()).Select(o => o.OldSystemApplicationId).ToList();
        applicationDTOs.RemoveAll(m => existingApplicationIds.Contains(m.OldSystemApplicationId));

        var addedApplications = await repository.AddApplications(applicationDTOs);
        logger.LogInformation("Added {Count:#,##0} applications", addedApplications.Count()); 
#endif

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}