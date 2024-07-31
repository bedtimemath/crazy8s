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

        /*** ADDRESSES ***/
        var addressDTOs = (await oldSystemService.GetAddresses())
            .Select(mapper.Map<AddressSql, AddressDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} addresses", addressDTOs.Count);

        /*** ORGANIZATIONS ***/
        var organizationDTOs = (await oldSystemService.GetOrganizations())
            .Select(mapper.Map<OrganizationSql, OrganizationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} organizations", organizationDTOs.Count);

        var existingOrgIds = (await repository.GetOrganizations()).Select(o => o.OldSystemOrganizationId).ToList();
        organizationDTOs.RemoveAll(m => existingOrgIds.Contains(m.OldSystemOrganizationId));
        
        /*** JOIN ADDRESSES TO ORGANIZATIONS ***/
        var totalOrganizations = organizationDTOs.Count;
        ConsoleEx.StartProgress("Joining addresses with organizations: ");
        for (int index = 0; index < totalOrganizations; index++)
        {
            var organization = organizationDTOs[index];
            var address = addressDTOs
                .FirstOrDefault(a => a.OldSystemUsaPostalId == organization.OldSystemPostalAddressId);
            if (address == null)
                throw new Exception($"Could not find address ({organization.OldSystemPostalAddressId}) for organization ({organization.OldSystemOrganizationId})");

            organization.Address = address;
            address.Organization = organization;

            ConsoleEx.ShowProgress((float)index / (float)totalOrganizations);
        }
        ConsoleEx.EndProgress();

        var addedOrgs = await repository.AddOrganizations(organizationDTOs);
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

        var coachesWithoutOrg = (await repository.GetCoaches())
            .Where(c => c.OrganizationId == null && c.OldSystemOrganizationId != null)
            .ToList();

        var totalCoachesWithoutOrg = coachesWithoutOrg.Count;
        var coachesUpdated = 0;

        ConsoleEx.StartProgress("Joining coaches with organizations: ");
        for (int index = 0; index < totalCoachesWithoutOrg; index++)
        {
            var coach = coachesWithoutOrg[index];
            if (coach.OldSystemOrganizationId.HasValue)
            {
                var organization = allOrganizations.FirstOrDefault(
                                       o => o.OldSystemOrganizationId == coach.OldSystemOrganizationId.Value) ??
                                   throw new Exception($"Could not find organization: {coach.OldSystemOrganizationId.Value}");
                if (coach.OrganizationId == null)
                {
                    coach.OrganizationId = organization.OrganizationId;

                    await repository.UpdateCoach(coach);
                    coachesUpdated++;
                }
            }

            ConsoleEx.ShowProgress((float)index / (float)totalCoachesWithoutOrg);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} coaches updated.", coachesUpdated);

        /*** APPLICATIONS ***/
        var applicationDTOs = (await oldSystemService.GetApplications())
            .Select(mapper.Map<ApplicationSql, ApplicationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} applications", applicationDTOs.Count);

        var existingApplicationIds = (await repository.GetApplications()).Select(o => o.OldSystemApplicationId).ToList();
        if (existingApplicationIds.Count > 0)
            logger.LogInformation("Removing {Count:#,##0} existing applications", existingApplicationIds.Count);
        applicationDTOs.RemoveAll(m => existingApplicationIds.Contains(m.OldSystemApplicationId));

        /*** APPLICATION CLUBS ***/
        var applicationClubDTOs = (await oldSystemService.GetApplicationClubs())
           .Select(mapper.Map<ApplicationClubSql, ApplicationClubDTO>)
           .ToList();

        logger.LogInformation("Found {Count:#,##0} application clubs", applicationClubDTOs.Count);

        var existingApplicationClubIds = (await repository.GetApplicationClubs()).Select(o => o.OldSystemApplicationClubId).ToList();
        if (existingApplicationClubIds.Count > 0)
            logger.LogInformation("Removing {Count:#,##0} existing application clubs", existingApplicationClubIds.Count);
        applicationClubDTOs.RemoveAll(m => existingApplicationClubIds.Contains(m.OldSystemApplicationClubId));

        /*** JOIN APPLICATIONS TO CLUBS ***/
        var totalClubs = applicationClubDTOs.Count;
        ConsoleEx.StartProgress("Joining addresses with organizations: ");
        for (int index = 0; index < totalClubs; index++)
        {
            var appClub = applicationClubDTOs[index];
            var application = applicationDTOs
                .FirstOrDefault(a => a.OldSystemApplicationId == appClub.OldSystemApplicationId);
            if (application == null)
                throw new Exception($"Could not find application ({appClub.OldSystemApplicationId}) for club ({appClub.OldSystemApplicationClubId})");

            application.ApplicationClubs ??= new List<ApplicationClubDTO>();
            application.ApplicationClubs.Add(appClub);

            ConsoleEx.ShowProgress((float)index / (float)totalClubs);
        }
        ConsoleEx.EndProgress();

        var addedApplications = await repository.AddApplications(applicationDTOs);
        logger.LogInformation("Added {Count:#,##0} applications", addedApplications.Count());

        /*** JOIN APPLICATIONS TO COACHES ***/
        var allCoaches = (await repository.GetCoaches()).ToList();

        var unlinkedCoachApps = (await repository.GetApplications())
            .Where(a => a is { LinkedCoachId: null, OldSystemLinkedCoachId: not null, IsCoachRemoved: false })
            .ToList();
        logger.LogInformation("Found {Count:#,##0} applications missing linked coaches", unlinkedCoachApps.Count);

        var totalUnlinkedCoachApps = unlinkedCoachApps.Count;
        var appsMissingCoach = 0;
        var appsLinkedToCoach = 0;

        ConsoleEx.StartProgress("Joining applications with coaches: ");
        for (int index = 0; index < totalUnlinkedCoachApps; index++)
        {
            var application = unlinkedCoachApps[index];
            var oldCoachLink = application.OldSystemLinkedCoachId;
            if (oldCoachLink == null) continue;

            var coach = allCoaches.FirstOrDefault(
                                   a => a.OldSystemCoachId == oldCoachLink.Value);
            if (coach == null)
            {
                application.IsCoachRemoved = true;
                appsMissingCoach++;
            }
            else
            {
                application.LinkedCoachId = coach.CoachId;
                appsLinkedToCoach++;
            }

            await repository.UpdateApplication(application);

            ConsoleEx.ShowProgress((float)index / (float)totalUnlinkedCoachApps);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} applications updated with coach link; {Missing:#,##0} missing.", appsLinkedToCoach, appsMissingCoach);

        /*** JOIN APPLICATIONS TO ORGANIZATIONS ***/
        var unlinkedOrganizationApps = (await repository.GetApplications())
            .Where(a => a is { LinkedOrganizationId: null, OldSystemLinkedOrganizationId: not null, IsOrganizationRemoved: false })
            .ToList();
        logger.LogInformation("Found {Count:#,##0} applications missing linked organizations", unlinkedOrganizationApps.Count);

        var totalUnlinkedOrganizationApps = unlinkedOrganizationApps.Count;
        var appsMissingOrganization = 0;
        var appsLinkedToOrganization = 0;

        ConsoleEx.StartProgress("Joining applications with organizations: ");
        for (int index = 0; index < totalUnlinkedOrganizationApps; index++)
        {
            var application = unlinkedOrganizationApps[index];
            var oldOrganizationLink = application.OldSystemLinkedOrganizationId;
            if (oldOrganizationLink == null) continue;

            var organization = allOrganizations.FirstOrDefault(
                                   a => a.OldSystemOrganizationId == oldOrganizationLink.Value);
            if (organization == null)
            {
                application.IsOrganizationRemoved = true;
                appsMissingOrganization++;
            }
            else
            {
                application.LinkedOrganizationId = organization.OrganizationId;
                appsLinkedToOrganization++;
            }

            await repository.UpdateApplication(application);
            appsLinkedToOrganization++;

            ConsoleEx.ShowProgress((float)index / (float)totalUnlinkedOrganizationApps);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} applications updated with organization link; {Missing:#,##0} missing.", appsLinkedToOrganization, appsMissingOrganization);

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}