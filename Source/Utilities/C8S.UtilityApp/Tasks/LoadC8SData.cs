using System.Diagnostics;
using AutoMapper;
using Azure.Core;
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
        var sqlAddresses = (await oldSystemService.GetAddresses()).ToList();

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

                if ((index + 1) % SaveBlock == 0)
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

                if ((index + 1) % SaveBlock == 0)
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

                if ((index + 1) % SaveBlock == 0)
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

                if ((index + 1) % SaveBlock == 0)
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

            if ((index + 1) % SaveBlock == 0)
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

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)unlinkedRequestPlacesCount);
        }
        ConsoleEx.EndProgress();
        await dbContext.SaveChangesAsync();

        logger.LogInformation("{Count:#,##0} requests updated with place.", placesLinked);

        /*** CLUBS ***/
        var sqlClubs = (await oldSystemService.GetClubs())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} clubs", sqlClubs.Count);

        var existingClubIds = await dbContext.Clubs.Select(o => o.OldSystemClubId).ToListAsync();
        sqlClubs.RemoveAll(m => existingClubIds.Contains(m.OldSystemClubId));

        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlApplications.Count);

        var sqlClubsCount = sqlClubs.Count;
        ConsoleEx.StartProgress("Adding clubs, plus persons & places to clubs: ");
        var clubsAdded = 0;
        for (int index = 0; index < sqlClubsCount; index++)
        {
            var sqlClub = sqlClubs[index];

            // start by finding the original coach
            var foundPerson = await dbContext.Persons
                .FirstOrDefaultAsync(c => c.OldSystemCoachId == sqlClub.OldSystemCoachId);
            if (foundPerson == null && sqlClub.OldSystemCoachId != null)
            {
                var deletedPerson = (await oldSystemService.GetDeletedCoach(sqlClub.OldSystemCoachId.Value));
                if (deletedPerson != null)
                {
                    foundPerson = await dbContext.Persons
                        .FirstOrDefaultAsync(c => c.Email == deletedPerson.Email);
                }
            }
            var person = foundPerson ??
                         throw new Exception($"Could not find person ({sqlClub.OldSystemCoachId}) for club ({sqlClub.OldSystemClubId})");

            // then by finding the original place
            var place = (await dbContext.Places
                    .FirstOrDefaultAsync(c => c.OldSystemOrganizationId == sqlClub.OldSystemOrganizationId)) ??
                        throw new Exception($"Could not find organization ({sqlClub.OldSystemOrganizationId}) for club ({sqlClub.OldSystemClubId})");

            // now create the club itself
            var clubStatus = sqlClub.StartsOnDateTime == null ? ClubStatus.Archived :
                sqlClub.StartsOnDateTime < DateTime.Today ? ClubStatus.Complete : ClubStatus.Active;
            var club = new ClubDb()
            {
                OldSystemClubId = sqlClub.OldSystemClubId,
                OldSystemOrganizationId = sqlClub.OldSystemOrganizationId,
                OldSystemCoachId = sqlClub.OldSystemCoachId,
                OldSystemMeetingAddressId = sqlClub.OldSystemMeetingAddressId,
                Status = clubStatus,
                Season = sqlClub.Season,
                AgeLevel = sqlClub.AgeLevel,
                ClubSize = sqlClub.ClubSize ?? ClubSize.Size16,
                StartsOn = sqlClub.StartsOn,
                ClubPersons = new List<ClubPersonDb>()
            };
            if (!String.IsNullOrEmpty(sqlClub.Notes))
                club.Notes = new List<ClubNoteDb>
                {
                    new()
                    {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlClub.Notes
                    }
                };
            dbContext.Clubs.Add(club);

            // and add the place and person
            club.Place = place;
            club.ClubPersons.Add(new ClubPersonDb()
            {
                Person = person,
                Club = club,
                IsPrimary = true
            });

            clubsAdded++;

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)sqlClubsCount);
        }
        ConsoleEx.EndProgress();

        logger.LogInformation("Added {Count:#,##0} clubs", clubsAdded);

        /*** SKUS ***/
        var sqlSkus = (await oldSystemService.GetSkus()).ToList();

        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        var existingSkuIds = await dbContext.Skus.Select(o => o.OldSystemSkuId).ToListAsync();
        sqlSkus.RemoveAll(m => existingSkuIds.Contains(m.OldSystemSkuId));

        logger.LogInformation("Removed existing, now {Count:#,##0} skus", sqlSkus.Count);

        if (sqlSkus.Count > 0)
        {
            /*** ADDING SKUS ***/
            var skusCount = sqlSkus.Count;
            ConsoleEx.StartProgress("Adding skus: ");
            var skusAdded = 0;
            for (int index = 0; index < skusCount; index++)
            {
                var sqlSku = sqlSkus[index];

                var sku = new SkuDb()
                {
                    OldSystemSkuId = sqlSku.OldSystemSkuId,
                    Key = sqlSku.Key,
                    Name = sqlSku.Name,
                    Status = sqlSku.Status ?? SkuStatus.Inactive,
                    Season = sqlSku.Season,
                    AgeLevel = sqlSku.AgeLevel,
                    ClubSize = sqlSku.ClubSize ?? ClubSize.Size16,
                    Comments = sqlSku.Notes
                };
                dbContext.Skus.Add(sku);

                skusAdded++;

                if ((index + 1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)skusCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} skus", skusAdded);
        }

        /*** ORDERS ***/
        var sqlOrders = (await oldSystemService.GetOrders()).ToList();
        var sqlOrderTrackers = (await oldSystemService.GetOrderTrackers()).ToList();

        logger.LogInformation("Found {Orders:#,##0} orders with {Trackers:#,##0} trackers",
            sqlOrders.Count, sqlOrderTrackers.Count);

        var existingOrderIds = await dbContext.Orders.Select(o => o.OldSystemOrderId).ToListAsync();
        sqlOrders.RemoveAll(m => existingOrderIds.Contains(m.OldSystemOrderId));

        logger.LogInformation("Removed existing, now {Count:#,##0} orders", sqlOrders.Count);

        if (sqlOrders.Count > 0)
        {
            var sqlOrdersCount = sqlOrders.Count;
            ConsoleEx.StartProgress("Adding orders & shipments, plus persons & places to orders: ");
            var ordersAdded = 0;
            for (int index = 0; index < sqlOrdersCount; index++)
            {
                var sqlOrder = sqlOrders[index];

                // start by finding the original club & address
                var orderClub = await dbContext.Clubs
                                    .FirstOrDefaultAsync(c => c.OldSystemClubId == sqlOrder.OldSystemClubId);
                var sqlAddress = sqlAddresses
                                     .FirstOrDefault(a => a.OldSystemUsaPostalId == sqlOrder.OldSystemShippingAddressId) ??
                                 throw new Exception($"Could not find address ({sqlOrder.OldSystemShippingAddressId}) for order ({sqlOrder.OldSystemOrderId})");

                // then the appropriate trackers
                var sqlTrackers = sqlOrderTrackers
                    .Where(t => t.OldSystemOrderId == sqlOrder.OldSystemOrderId)
                    .ToList();

                // then create the new order
                var status = sqlOrder.Status ??
                             throw new Exception($"Missing status for order ({sqlOrder.OldSystemOrderId})");
                var recipient = sqlAddress.BusinessName ?? sqlAddress.RecipientName ??
                    throw new Exception($"Missing recipient for order ({sqlOrder.OldSystemOrderId})");
                var line1 = sqlAddress.StreetAddress ??
                                  throw new Exception($"Missing street address for order ({sqlOrder.OldSystemOrderId})");
                var city = sqlAddress.City ??
                                  throw new Exception($"Missing city for order ({sqlOrder.OldSystemOrderId})");
                var state = sqlAddress.State ??
                                  throw new Exception($"Missing state for order ({sqlOrder.OldSystemOrderId})");
                var zipCode = sqlAddress.PostalCode ??
                                  throw new Exception($"Missing ZIP for order ({sqlOrder.OldSystemOrderId})");
                var order = new OrderDb()
                {
                    OldSystemOrderId = sqlOrder.OldSystemOrderId,
                    OldSystemClubId = sqlOrder.OldSystemClubId,
                    OldSystemShippingAddressId = sqlOrder.OldSystemShippingAddressId,
                    Number = sqlOrder.Number,
                    Status = status,
                    ContactName = sqlAddress.RecipientName,
                    ContactEmail = sqlOrder.ContactEmail,
                    ContactPhone = sqlOrder.ContactPhone,
                    Recipient = recipient,
                    Line1 = line1,
                    City = city,
                    State = state,
                    ZIPCode = zipCode,
                    IsMilitary = sqlAddress.IsMilitary,
                    OrderedOn = sqlOrder.OrderedOn,
                    ArriveBy = sqlOrder.ArriveBy,
                    ShippedOn = sqlOrder.ShippedOn,
                    EmailedOn = sqlOrder.EmailedOn,
                    Club = orderClub,
                    Shipments = sqlTrackers.Select(t => new ShipmentDb()
                    {
                        ShipMethod = t.Method,
                        TrackingNumber = t.Code
                    }).ToList(),
                    Notes = String.IsNullOrEmpty(sqlOrder.Notes) ? [] :
                        new List<OrderNoteDb>() {
                            new() { Author = SoftCrowConstants.Display.System, Content = sqlOrder.Notes }
                        }
                };
                dbContext.Orders.Add(order);

                ordersAdded++;

                if ((index + 1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();

                ConsoleEx.ShowProgress((float)index / (float)sqlOrdersCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("Added {Count:#,##0} orders", ordersAdded);

        }
        /*** ORDER SKUS ***/
        var sqlOrderSkus = (await oldSystemService.GetOrderSkus())
            .ToList();

        logger.LogInformation("Found {Count:#,##0} order skus", sqlOrderSkus.Count);

        var existingOrderSkuIds = await dbContext.OrderSkus.Select(o => o.OldSystemOrderSkuId).ToListAsync();
        sqlOrderSkus.RemoveAll(m => existingOrderSkuIds.Contains(m.OldSystemOrderSkuId));

        logger.LogInformation("Removed existing, now {Count:#,##0} order skus", sqlOrderSkus.Count);

        /*** JOIN ORDERS & SKUS TO ORDER SKUS ***/
        var sqlOrderSkusCount = sqlOrderSkus.Count;
        ConsoleEx.StartProgress("Joining orders & skus with order skus: ");
        var addedOrderSkus = 0;
        var skippedOrderSkus = 0;
        for (int index = 0; index < sqlOrderSkusCount; index++)
        {
            var sqlOrderSku = sqlOrderSkus[index];

            var order = dbContext.Orders
                .FirstOrDefault(o => o.OldSystemOrderId == sqlOrderSku.OldSystemOrderId);
            if (order == null) { skippedOrderSkus++; continue; }

            var sku = dbContext.Skus
                .FirstOrDefault(o => o.OldSystemSkuId == sqlOrderSku.OldSystemSkuId);
            if (sku == null) { skippedOrderSkus++; continue; }

            var orderSku = new OrderSkuDb()
            {
                OldSystemOrderSkuId = sqlOrderSku.OldSystemOrderSkuId,
                OldSystemOrderId = sqlOrderSku.OldSystemOrderId,
                OldSystemSkuId = sqlOrderSku.OldSystemSkuId,
                Ordinal = sqlOrderSku.Ordinal,
                Quantity = sqlOrderSku.Quantity,
                Order = order,
                Sku = sku
            };
            dbContext.OrderSkus.Add(orderSku);

            addedOrderSkus++;

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)sqlOrderSkusCount);
        }
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        logger.LogInformation("Added {Count:#,##0} orderSkus; skipped {Skipped:#,##0}", addedOrderSkus, skippedOrderSkus);  

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}