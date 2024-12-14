using AutoMapper;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common;

namespace C8S.UtilityApp.Tasks;

internal class LoadC8SData(
    ILogger<LoadC8SData> logger,
    LoadC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    IMapper mapper,
    OldSystemService oldSystemService)
    : IActionLauncher
{
    private const int SaveBlock = 500;

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

        var dbContext = await dbContextFactory.CreateDbContextAsync();

        /*** ADDRESSES ***/
        var sqlAddresses = (await oldSystemService.GetAddresses())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} addresses", sqlAddresses.Count);

        /*** ORGANIZATIONS ***/
        var sqlOrganizations = (await oldSystemService.GetOrganizations())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} organizations", sqlOrganizations.Count);

        var existingOrgIds = await dbContext.Places.Select(o => o.OldSystemOrganizationId).ToListAsync();
        sqlOrganizations.RemoveAll(m => existingOrgIds.Contains(m.OldSystemOrganizationId));

        logger.LogInformation("Removed existing, now {Count:#,##0} organizations", sqlOrganizations.Count);

        if (sqlOrganizations.Count > 0)
        {
            /*** ADDING PLACES ***/
            var sqlOrganizationsCount = sqlOrganizations.Count;
            ConsoleEx.StartProgress("Adding places: ");
            for (int index = 0; index < sqlOrganizationsCount; index++)
            {
                var sqlOrganization = sqlOrganizations[index];
                var sqlAddress = sqlAddresses
                    .FirstOrDefault(a => a.OldSystemUsaPostalId == sqlOrganization.OldSystemPostalAddressId);
                if (sqlAddress == null)
                    throw new Exception($"Could not find address ({sqlOrganization.OldSystemPostalAddressId}) for organization ({sqlOrganization.OldSystemOrganizationId})");

                var place = new PlaceDb()
                {
                    OldSystemCompanyId = sqlOrganization.OldSystemCompanyId,
                    OldSystemOrganizationId = sqlOrganization.OldSystemOrganizationId,
                    OldSystemPostalAddressId = sqlOrganization.OldSystemPostalAddressId,
                    OldSystemUsaPostalId = sqlAddress.OldSystemUsaPostalId,
                    Name = sqlOrganization.Name ?? SoftCrowConstants.Display.NotSet,
                    Type = sqlOrganization.Type,
                    TypeOther = sqlOrganization.TypeOther,
                    TaxIdentifier = sqlOrganization.TaxIdentifier,
                    Line1 = sqlAddress.StreetAddress ?? SoftCrowConstants.Display.NotSet,
                    Line2 = null,
                    City = sqlAddress.City ?? SoftCrowConstants.Display.NotSet,
                    State = sqlAddress.State ?? SoftCrowConstants.Display.NotSet,
                    ZIPCode = sqlAddress.PostalCode ?? SoftCrowConstants.Display.NotSet,
                    IsMilitary = sqlAddress.IsMilitary,
                    CreatedOn = sqlAddress.CreatedOn
                };
                if (!String.IsNullOrEmpty(sqlOrganization.Notes))
                    place.Notes = new List<PlaceNoteDb>
                {
                    new()
                    {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlOrganization.Notes
                    }
                };
                dbContext.Places.Add(place);

                if ((index+1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)sqlOrganizationsCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} places", sqlOrganizationsCount);

        }

        /*** COACHES ***/
        var sqlCoaches = (await oldSystemService.GetCoaches())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} coaches", sqlCoaches.Count);

        var hasOrgIds = sqlCoaches.Where(c => c.OldSystemOrganizationId.HasValue).ToList();
        logger.LogInformation("Found {Count:#,##0} coaches with org ids", hasOrgIds.Count);

        var existingCoachIds = await dbContext.Persons.Select(o => o.OldSystemCoachId).ToListAsync();
        sqlCoaches.RemoveAll(m => existingCoachIds.Contains(m.OldSystemCoachId));

        logger.LogInformation("Removed existing, now {Count:#,##0} coaches", sqlCoaches.Count);

        if (sqlCoaches.Count > 0)
        {            
            /*** ADDING PERSONS ***/
            var sqlCoachesCount = sqlCoaches.Count;
            ConsoleEx.StartProgress("Adding persons: ");
            for (int index = 0; index < sqlCoachesCount; index++)
            {
                var sqlCoach = sqlCoaches[index];

                var person = new PersonDb()
                {
                    OldSystemCoachId = sqlCoach.OldSystemCoachId,
                    OldSystemOrganizationId = sqlCoach.OldSystemOrganizationId,
                    OldSystemUserId = sqlCoach.OldSystemUserId,
                    OldSystemCompanyId = sqlCoach.OldSystemCompanyId,
                    FirstName = sqlCoach.FirstName,
                    LastName = sqlCoach.LastName,
                    Email = sqlCoach.Email,
                    TimeZone = sqlCoach.TimeZone,
                    Phone = sqlCoach.Phone +
                            (String.IsNullOrEmpty(sqlCoach.PhoneExt) ? null : $" x{sqlCoach.PhoneExt}"),
                    CreatedOn = sqlCoach.CreatedOn
                };
                if (!String.IsNullOrEmpty(sqlCoach.Notes))
                    person.Notes = new List<PersonNoteDb>
                    {
                        new()
                        {
                            Author = SoftCrowConstants.Display.System,
                            Content = sqlCoach.Notes
                        }
                    };
                dbContext.Persons.Add(person);

                if ((index+1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)sqlCoachesCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} persons", sqlCoachesCount);
        }

        /*** JOINING PERSONS WITH PLACES ***/
        var allPlaces = await dbContext.Places.ToListAsync();

        var personsNoPlace = await dbContext.Persons
            .Where(c => c.PlaceId == null && c.OldSystemOrganizationId != null)
            .ToListAsync();

        var personsNoPlaceCount = personsNoPlace.Count;
        var personsUpdated = 0;
        logger.LogInformation("Found {Count:#,##0} persons without a place", personsNoPlaceCount);

        if (personsNoPlaceCount > 0)
        {
            ConsoleEx.StartProgress("Joining persons with places: ");
            for (int index = 0; index < personsNoPlaceCount; index++)
            {
                var person = personsNoPlace[index];
                if (person.OldSystemOrganizationId.HasValue)
                {
                    var place = allPlaces.FirstOrDefault(
                                    o => o.OldSystemOrganizationId == person.OldSystemOrganizationId.Value) ??
                                throw new Exception($"Could not find organization: {person.OldSystemOrganizationId.Value}");
                    if (person.PlaceId == null)
                    {
                        person.PlaceId = place.PlaceId;
                        personsUpdated++;
                    }
                }

                ConsoleEx.ShowProgress((float)index / (float)personsNoPlaceCount);
            }
            ConsoleEx.EndProgress();
            
            await dbContext.SaveChangesAsync();
            logger.LogInformation("{Count:#,##0} persons updated with a place.", personsUpdated);
        }

        /*** APPLICATIONS ***/
        var sqlApplications = (await oldSystemService.GetApplications())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} applications", sqlApplications.Count);

        var existingRequestIds = await dbContext.Requests.Select(o => o.OldSystemApplicationId).ToListAsync();
        sqlApplications.RemoveAll(m => existingRequestIds.Contains(m.OldSystemApplicationId));

        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlApplications.Count);

        if (sqlApplications.Count > 0)
        {            
            /*** ADDING REQUESTS ***/
            var sqlApplicationsCount = sqlApplications.Count;
            ConsoleEx.StartProgress("Adding requests: ");
            for (int index = 0; index < sqlApplicationsCount; index++)
            {
                var sqlApplication = sqlApplications[index];

                var request = new RequestDb()
                {
                    OldSystemApplicationId = sqlApplication.OldSystemApplicationId,
                    OldSystemAddressId = sqlApplication.OldSystemAddressId,
                    OldSystemLinkedCoachId = sqlApplication.OldSystemLinkedCoachId,
                    OldSystemLinkedOrganizationId = sqlApplication.OldSystemLinkedOrganizationId,
                    Status = sqlApplication.Status ?? throw new Exception("Missing Status"),
                    PersonType = sqlApplication.ApplicantType,
                    PersonFirstName = sqlApplication.ApplicantFirstName,
                    PersonLastName = sqlApplication.ApplicantLastName ?? throw new Exception("Missing Last Name"),
                    PersonEmail = sqlApplication.ApplicantEmail ?? throw new Exception("Missing Email"),
                    PersonTimeZone = sqlApplication.ApplicantTimeZone ?? throw new Exception("Missing Time Zone"),
                    PersonPhone = sqlApplication.ApplicantPhone +
                                  (String.IsNullOrEmpty(sqlApplication.ApplicantPhoneExt) ? null : $" x{sqlApplication.ApplicantPhoneExt}"),
                    PlaceName = sqlApplication.OrganizationName,
                    PlaceType = sqlApplication.OrganizationType,
                    PlaceTypeOther = sqlApplication.OrganizationTypeOther,
                    PlaceTaxIdentifier = sqlApplication.OrganizationTaxIdentifier,
                    WorkshopCode = sqlApplication.WorkshopCode,
                    ReferenceSource = null,
                    ReferenceSourceOther = null,
                    Comments = sqlApplication.Comments,
                    SubmittedOn = sqlApplication.SubmittedOn ?? throw new Exception("Missing Submitted On"),
                    CreatedOn = sqlApplication.CreatedOn
                };
                if (!String.IsNullOrEmpty(sqlApplication.Notes))
                    request.Notes = new List<RequestNoteDb>
                    {
                        new()
                        {
                            Author = SoftCrowConstants.Display.System,
                            Content = sqlApplication.Notes
                        }
                    };
                dbContext.Requests.Add(request);

                if ((index+1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} requests", sqlApplicationsCount);
        }

        /*** APPLICATION CLUBS ***/
        var sqlApplicationClubs = (await oldSystemService.GetApplicationClubs())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        var existingApplicationClubIds = await dbContext.RequestedClubs
            .Select(o => o.OldSystemApplicationClubId)
            .ToListAsync();
        sqlApplicationClubs.RemoveAll(m => existingApplicationClubIds.Contains(m.OldSystemApplicationClubId));

        logger.LogInformation("Removed existing, now {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        if (sqlApplicationClubs.Count > 0)
        {
            /*** ADDING REQUESTED CLUBS ***/
            var applicationClubsCount = sqlApplicationClubs.Count;
            ConsoleEx.StartProgress("Adding requested clubs: ");
            var requestedClubsAdded = 0;
            for (int index = 0; index < applicationClubsCount; index++)
            {
                var appClub = sqlApplicationClubs[index];
                var request = await dbContext.Requests.FirstOrDefaultAsync(r =>
                        r.OldSystemApplicationId == appClub.OldSystemApplicationId);

                // some point to missing / incomplete applications
                if (request != null)
                {
                    var requestedClub = new RequestedClubDb()
                    {
                        RequestId = request.RequestId,
                        OldSystemApplicationClubId = appClub.OldSystemApplicationClubId,
                        OldSystemApplicationId = appClub.OldSystemApplicationId,
                        OldSystemLinkedClubId = appClub.OldLinkedClubId,
                        AgeLevel = appClub.AgeLevel ?? throw new Exception("Missing Age Level"),
                        ClubSize = appClub.ClubSize ?? ClubSize.Size16,
                        Season = appClub.Season ?? throw new Exception("Missing Season"),
                        StartsOn = appClub.StartsOn ?? throw new Exception("Missing Starts On")
                    };
                    dbContext.RequestedClubs.Add(requestedClub);
                    requestedClubsAdded++;
                }

                if ((index+1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)applicationClubsCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} request clubs", requestedClubsAdded);
        }

        /*** JOIN REQUESTS TO PERSONS & PLACES ***/
        var unlinkedRequestPersons = await dbContext.Requests
            .Where(r => r.PersonId == null && r.OldSystemLinkedCoachId != null)
            .ToListAsync();

        var unlinkedRequestPersonsCount = unlinkedRequestPersons.Count;
        logger.LogInformation("Found {Count:#,##0} requests missing persons.", 
            unlinkedRequestPersonsCount);

        ConsoleEx.StartProgress("Joining requests with persons: ");
        var personsLinked = 0;
        for (int index = 0; index < unlinkedRequestPersonsCount; index++)
        {
            var request = unlinkedRequestPersons[index];

            var oldCoachLink = request.OldSystemLinkedCoachId;
            if (oldCoachLink != null)
            {
                var person = await dbContext.Persons.FirstOrDefaultAsync(
                    a => a.OldSystemCoachId == oldCoachLink.Value);
                if (person != null)
                {
                    request.PersonId = person.PersonId;
                    personsLinked++;
                }
            }

            if ((index+1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)unlinkedRequestPersonsCount);
        }
        ConsoleEx.EndProgress();
        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Count:#,##0} requests updated with person.", personsLinked);

        /*** JOIN REQUESTS TO PLACES ***/
        var unlinkedRequestPlaces = await dbContext.Requests
            .Where(r => r.PlaceId == null && r.OldSystemLinkedOrganizationId != null)
            .ToListAsync();

        var unlinkedRequestPlacesCount = unlinkedRequestPlaces.Count;
        logger.LogInformation("Found {Count:#,##0} requests missing places.", unlinkedRequestPlacesCount);

        ConsoleEx.StartProgress("Joining requests with places: ");
        var placesLinked = 0;
        for (int index = 0; index < unlinkedRequestPlacesCount; index++)
        {
            var request = unlinkedRequestPlaces[index];

            var oldOrganizationLink = request.OldSystemLinkedOrganizationId;
            if (oldOrganizationLink != null)
            {
                var place = await dbContext.Places.FirstOrDefaultAsync(
                    a => a.OldSystemOrganizationId == oldOrganizationLink.Value);
                if (place != null)
                {
                    request.PlaceId = place.PlaceId;
                    placesLinked++;
                }
            }

            if ((index+1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)unlinkedRequestPlacesCount);
        }
        ConsoleEx.EndProgress();
        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Count:#,##0} requests updated with place.", placesLinked);


#if false
        /*** JOIN APPLICATIONS TO ORGANIZATIONS ***/
        var unlinkedOrganizationApps = (await GetApplications())
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

            await UpdateApplication(application);
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

        var existingClubIds = (await GetClubs()).Select(o => o.OldSystemClubId).ToList();
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

        var addedClubs = await AddClubs(sqlClubs);
        logger.LogInformation("Added {Count:#,##0} clubs", addedClubs.Count());

        /*** SKUS ***/
        var sqlSkus = (await oldSystemService.GetSkus())
            .Select(mapper.Map<SkuSql, SkuDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        var existingSkuIds = (await GetSkus()).Select(o => o.OldSystemSkuId).ToList();
        sqlSkus.RemoveAll(m => existingSkuIds.Contains(m.OldSystemSkuId));

        var addedSkus = await AddSkus(sqlSkus);
        logger.LogInformation("Added {Count:#,##0} skus", addedSkus.Count());

        /*** ORDERS ***/
        var sqlOrders = (await oldSystemService.GetOrders())
            .Select(mapper.Map<OrderSql, OrderDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} orders", sqlOrders.Count);

        var existingOrderIds = (await GetOrders()).Select(o => o.OldSystemOrderId).ToList();
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

        var addedOrders = await AddOrders(sqlOrders);
        logger.LogInformation("Added {Count:#,##0} orders", addedOrders.Count());

        /*** ORDER SKUS ***/
        var sqlOrderSkus = (await oldSystemService.GetOrderSkus())
            .Select(mapper.Map<OrderSkuSql, OrderSkuDTO>)
            .ToList();

        logger.LogInformation("Found {Count:#,##0} order skus", sqlOrderSkus.Count);

        var existingOrderSkuIds = (await GetOrderSkus()).Select(o => o.OldSystemOrderSkuId).ToList();
        sqlOrderSkus.RemoveAll(m => existingOrderSkuIds.Contains(m.OldSystemOrderSkuId));

        // we remove the ones we're not adding, so we need to get these lists again
        var dtoOrders = (await GetOrders()).ToList();
        var dtoSkus = (await GetSkus()).ToList();

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

        var addedOrderSkus = await AddOrderSkus(sqlOrderSkus);
        logger.LogInformation("Added {Count:#,##0} orderSkus; skipped {Skipped:#,##0}", addedOrderSkus.Count(), skippedOrderSkus.Count);  
#endif

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}