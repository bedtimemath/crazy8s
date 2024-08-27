using AutoMapper;
using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Contexts;
using C8S.Database.Repository.Repositories;
using C8S.UtilityApp.Base;
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
        var sqlAddresses = (await oldSystemService.GetAddresses())
            .Select(mapper.Map<AddressSql, AddressDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} addresses", sqlAddresses.Count);
        /*** ORGANIZATIONS ***/
        var sqlOrganizations = (await oldSystemService.GetOrganizations())
            .Select(mapper.Map<OrganizationSql, OrganizationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} organizations", sqlOrganizations.Count);

        var existingOrgIds = (await repository.GetOrganizations()).Select(o => o.OldSystemOrganizationId).ToList();
        sqlOrganizations.RemoveAll(m => existingOrgIds.Contains(m.OldSystemOrganizationId));

        /*** JOIN ADDRESSES TO ORGANIZATIONS ***/
        var sqlOrganizationsCount = sqlOrganizations.Count;
        ConsoleEx.StartProgress("Joining addresses with organizations: ");
        for (int index = 0; index < sqlOrganizationsCount; index++)
        {
            var organization = sqlOrganizations[index];
            var address = sqlAddresses
                .FirstOrDefault(a => a.OldSystemUsaPostalId == organization.OldSystemPostalAddressId);
            if (address == null)
                throw new Exception($"Could not find address ({organization.OldSystemPostalAddressId}) for organization ({organization.OldSystemOrganizationId})");

            organization.Address = address;
            address.Organization = organization;

            ConsoleEx.ShowProgress((float)index / (float)sqlOrganizationsCount);
        }
        ConsoleEx.EndProgress();

        var addedOrgs = await repository.AddOrganizations(sqlOrganizations);
        logger.LogInformation("Added {Count:#,##0} organizations", addedOrgs.Count());

        /*** COACHES ***/
        var sqlCoaches = (await oldSystemService.GetCoaches())
            .Select(mapper.Map<CoachSql, CoachDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} coaches", sqlCoaches.Count);
        
        var hasOrgIds = sqlCoaches.Where(c => c.OldSystemOrganizationId.HasValue).ToList();
        logger.LogInformation("Found {Count:#,##0} coaches with org ids", hasOrgIds.Count);

        var existingCoachIds = (await repository.GetCoaches()).Select(o => o.OldSystemCoachId).ToList();
        sqlCoaches.RemoveAll(m => existingCoachIds.Contains(m.OldSystemCoachId));

        var addedCoaches = await repository.AddCoaches(sqlCoaches);
        logger.LogInformation("Added {Count:#,##0} coaches", addedCoaches.Count());

        /*** JOINING COACHES & ORGANIZATIONS ***/
        var allOrganizations = (await repository.GetOrganizations()).ToList();

        var coachesWithoutOrg = (await repository.GetCoaches())
            .Where(c => c.OrganizationId == null && c.OldSystemOrganizationId != null)
            .ToList();

        var coachesWithoutOrgCount = coachesWithoutOrg.Count;
        var coachesUpdated = 0;

        ConsoleEx.StartProgress("Joining coaches with organizations: ");
        for (int index = 0; index < coachesWithoutOrgCount; index++)
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

            ConsoleEx.ShowProgress((float)index / (float)coachesWithoutOrgCount);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} coaches updated.", coachesUpdated);

        /*** APPLICATIONS ***/
        var sqlApplications = (await oldSystemService.GetApplications())
            .Select(mapper.Map<ApplicationSql, ApplicationDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} applications", sqlApplications.Count);

        var existingApplicationIds = (await repository.GetApplications()).Select(o => o.OldSystemApplicationId).ToList();
        if (existingApplicationIds.Count > 0)
            logger.LogInformation("Skipping {Count:#,##0} existing applications", existingApplicationIds.Count);
        sqlApplications.RemoveAll(m => existingApplicationIds.Contains(m.OldSystemApplicationId));

        /*** APPLICATION CLUBS ***/
        var sqlApplicationClubs = (await oldSystemService.GetApplicationClubs())
           .Select(mapper.Map<ApplicationClubSql, ApplicationClubDTO>)
           .ToList();

        logger.LogInformation("Found {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        var existingApplicationClubIds = (await repository.GetApplicationClubs()).Select(o => o.OldSystemApplicationClubId).ToList();
        if (existingApplicationClubIds.Count > 0)
            logger.LogInformation("Skipping {Count:#,##0} existing application clubs", existingApplicationClubIds.Count);
        sqlApplicationClubs.RemoveAll(m => existingApplicationClubIds.Contains(m.OldSystemApplicationClubId));

        /*** JOIN APPLICATIONS TO CLUBS ***/
        var applicationClubsCount = sqlApplicationClubs.Count;
        ConsoleEx.StartProgress("Joining addresses with organizations: ");
        for (int index = 0; index < applicationClubsCount; index++)
        {
            var appClub = sqlApplicationClubs[index];
            var application = sqlApplications
                .FirstOrDefault(a => a.OldSystemApplicationId == appClub.OldSystemApplicationId);
            if (application == null)
                throw new Exception($"Could not find application ({appClub.OldSystemApplicationId}) for club ({appClub.OldSystemApplicationClubId})");

            application.ApplicationClubs ??= new List<ApplicationClubDTO>();
            application.ApplicationClubs.Add(appClub);

            ConsoleEx.ShowProgress((float)index / (float)applicationClubsCount);
        }
        ConsoleEx.EndProgress();

        var addedApplications = await repository.AddApplications(sqlApplications);
        logger.LogInformation("Added {Count:#,##0} applications", addedApplications.Count());

        /*** JOIN APPLICATIONS TO COACHES ***/
        var allCoaches = (await repository.GetCoaches()).ToList();

        var unlinkedCoachApps = (await repository.GetApplications())
            .Where(a => a is { LinkedCoachId: null, OldSystemLinkedCoachId: not null, IsCoachRemoved: false })
            .ToList();
        logger.LogInformation("Found {Count:#,##0} applications missing linked coaches", unlinkedCoachApps.Count);

        var unlinkedCoachAppsCount = unlinkedCoachApps.Count;
        var appsMissingCoach = 0;
        var appsLinkedToCoach = 0;

        ConsoleEx.StartProgress("Joining applications with coaches: ");
        for (int index = 0; index < unlinkedCoachAppsCount; index++)
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

            ConsoleEx.ShowProgress((float)index / (float)unlinkedCoachAppsCount);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} applications updated with coach link; {Missing:#,##0} missing.", appsLinkedToCoach, appsMissingCoach);

        /*** JOIN APPLICATIONS TO ORGANIZATIONS ***/
        var unlinkedOrganizationApps = (await repository.GetApplications())
            .Where(a => a is { LinkedOrganizationId: null, OldSystemLinkedOrganizationId: not null, IsOrganizationRemoved: false })
            .ToList();
        logger.LogInformation("Found {Count:#,##0} applications missing linked organizations", unlinkedOrganizationApps.Count);

        var unlinkedOrganizationAppsCount = unlinkedOrganizationApps.Count;
        var appsMissingOrganization = 0;
        var appsLinkedToOrganization = 0;

        ConsoleEx.StartProgress("Joining applications with organizations: ");
        for (int index = 0; index < unlinkedOrganizationAppsCount; index++)
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

            ConsoleEx.ShowProgress((float)index / (float)unlinkedOrganizationAppsCount);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("{Count:#,##0} applications updated with organization link; {Missing:#,##0} missing.", appsLinkedToOrganization, appsMissingOrganization);
        
        /*** CLUBS ***/
        var sqlClubs = (await oldSystemService.GetClubs())
            .Select(mapper.Map<ClubSql, ClubDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} clubs", sqlClubs.Count);

        var existingClubIds = (await repository.GetClubs()).Select(o => o.OldSystemClubId).ToList();
        sqlClubs.RemoveAll(m => existingClubIds.Contains(m.OldSystemClubId));

        var sqlClubsCount = sqlClubs.Count;
        ConsoleEx.StartProgress("Adding coaches, orgs and addresses to clubs: ");
        for (int index = 0; index < sqlClubsCount; index++)
        {
            var club = sqlClubs[index];

            var foundCoach = allCoaches.FirstOrDefault(c => c.OldSystemCoachId == club.OldSystemCoachId);
            if (foundCoach == null && club.OldSystemCoachId != null)
            {
                var deletedCoach = (await oldSystemService.GetDeletedCoach(club.OldSystemCoachId.Value));
                if (deletedCoach != null)
                {
                    foundCoach = allCoaches.FirstOrDefault(c => c.Email == deletedCoach.Email);
                }
            }
            club.CoachId = foundCoach?.Id ??
                throw new Exception($"Could not find coach ({club.OldSystemCoachId}) for club ({club.OldSystemClubId})");

            club.OrganizationId = allOrganizations.FirstOrDefault(c => c.OldSystemOrganizationId == club.OldSystemOrganizationId)?.Id ??
                                throw new Exception($"Could not find organization ({club.OldSystemOrganizationId}) for club ({club.OldSystemClubId})");

            var address = sqlAddresses
                .FirstOrDefault(a => a.OldSystemUsaPostalId == club.OldSystemMeetingAddressId);
            if (address == null)
                throw new Exception($"Could not find address ({club.OldSystemMeetingAddressId}) for club ({club.OldSystemClubId})");
            club.Address = address;

            ConsoleEx.ShowProgress((float)index / (float)sqlClubsCount);
        }
        ConsoleEx.EndProgress();

        var addedClubs = await repository.AddClubs(sqlClubs);
        logger.LogInformation("Added {Count:#,##0} clubs", addedClubs.Count()); 
        
        /*** SKUS ***/
        var sqlSkus = (await oldSystemService.GetSkus())
            .Select(mapper.Map<SkuSql, SkuDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        var existingSkuIds = (await repository.GetSkus()).Select(o => o.OldSystemSkuId).ToList();
        sqlSkus.RemoveAll(m => existingSkuIds.Contains(m.OldSystemSkuId));

        var addedSkus = await repository.AddSkus(sqlSkus);
        logger.LogInformation("Added {Count:#,##0} skus", addedSkus.Count());

        /*** ORDERS ***/
        var sqlOrders = (await oldSystemService.GetOrders())
            .Select(mapper.Map<OrderSql, OrderDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} orders", sqlOrders.Count);

        var existingOrderIds = (await repository.GetOrders()).Select(o => o.OldSystemOrderId).ToList();
        sqlOrders.RemoveAll(m => existingOrderIds.Contains(m.OldSystemOrderId));

        // we remove the ones we're not adding, so we need to get this list again
        sqlClubs = (await oldSystemService.GetClubs())
            .Select(mapper.Map<ClubSql, ClubDTO>)
            .ToList();

        /*** JOIN ADDRESSES & COACHES TO ORDERS ***/
        var sqlOrdersCount = sqlOrders.Count;
        ConsoleEx.StartProgress("Joining addresses & clubs with orders: ");
        for (int index = 0; index < sqlOrdersCount; index++)
        {
            var order = sqlOrders[index];

            var address = sqlAddresses
                .FirstOrDefault(a => a.OldSystemUsaPostalId == order.OldSystemShippingAddressId);
            if (address == null)
                throw new Exception($"Could not find address ({order.OldSystemShippingAddressId}) for order ({order.OldSystemOrderId})");

            order.Address = address;
            address.Order = order;

            if (order.OldSystemClubId != null)
            {
                var club = sqlClubs
                    .FirstOrDefault(a => a.OldSystemClubId == order.OldSystemClubId);
                if (club == null)
                    throw new Exception($"Could not find club ({order.OldSystemClubId}) for order ({order.OldSystemOrderId})");
            }

            ConsoleEx.ShowProgress((float)index / (float)sqlOrdersCount);
        }
        ConsoleEx.EndProgress();

        var addedOrders = await repository.AddOrders(sqlOrders);
        logger.LogInformation("Added {Count:#,##0} orders", addedOrders.Count());  

        /*** ORDER SKUS ***/
        var sqlOrderSkus = (await oldSystemService.GetOrderSkus())
            .Select(mapper.Map<OrderSkuSql, OrderSkuDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} order skus", sqlOrderSkus.Count);

        var existingOrderSkuIds = (await repository.GetOrderSkus()).Select(o => o.OldSystemOrderSkuId).ToList();
        sqlOrderSkus.RemoveAll(m => existingOrderSkuIds.Contains(m.OldSystemOrderSkuId));

        // we remove the ones we're not adding, so we need to get these lists again
        var dtoOrders = (await repository.GetOrders())
            .ToList();
        var dtoSkus = (await repository.GetSkus())
            .ToList();

        /*** JOIN ORDERS & SKUS TO ORDER SKUS ***/
        var skippedOrderSkus = new List<OrderSkuDTO>();
        var sqlOrderSkusCount = sqlOrderSkus.Count;
        ConsoleEx.StartProgress("Joining orders & skus with order skus: ");
        for (int index = 0; index < sqlOrderSkusCount; index++)
        {
            var orderSku = sqlOrderSkus[index];

            var order = dtoOrders
                .FirstOrDefault(o => o.OldSystemOrderId == orderSku.OldSystemOrderId);
            if (order == null)
            {
                skippedOrderSkus.Add(orderSku);
                continue;
            }
            
            orderSku.OrderId = order.Id;

            var sku = dtoSkus
                .FirstOrDefault(o => o.OldSystemSkuId == orderSku.OldSystemSkuId);
            if (sku == null)
            {
                skippedOrderSkus.Add(orderSku);
                continue;
            }

            orderSku.SkuId = sku.Id;

            //try { await repository.AddOrderSku(orderSku); }
            //catch (Exception ex) { logger.LogError(ex, "Could not add OrderSku: {@OrderSku}", orderSku); } 

            ConsoleEx.ShowProgress((float)index / (float)sqlOrderSkusCount);
        }
        ConsoleEx.EndProgress();

        var removedOrderSkuIds = skippedOrderSkus.Select(o => o.OldSystemOrderSkuId).ToList();
        sqlOrderSkus.RemoveAll(m => removedOrderSkuIds.Contains(m.OldSystemOrderSkuId));

        var addedOrderSkus = await repository.AddOrderSkus(sqlOrderSkus);
        logger.LogInformation("Added {Count:#,##0} orderSkus; skipped {Skipped:#,##0}", addedOrderSkus.Count(), skippedOrderSkus.Count);  

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}