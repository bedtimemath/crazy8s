using System.Diagnostics;
using System.Text.RegularExpressions;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using C8S.UtilityApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common;

namespace C8S.UtilityApp.Tasks;

internal class LoadC8SData(
    ILogger<LoadC8SData> logger,
    LoadC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    OldSystemService oldSystemService)
    : IActionLauncher
{
    private const string CoachTable = "Coach";
    private const string OrganizationTable = "Organization";
    private const string UserTable = "User";
    private const string CompanyTable = "Company";
    private const string PostalAddressTable = "PostalAddress";
    private const string UsaPostalTable = "UsaPostal";

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

        /*** ADDRESSES ***/
        var sqlAddresses = (await oldSystemService.GetAddresses()).ToList();
        logger.LogInformation("Found {Count:#,##0} addresses", sqlAddresses.Count);

        /*** ORGANIZATIONS ***/
        var sqlOrganizations = (await oldSystemService.GetOrganizations()).ToList();
        logger.LogInformation("Found {Count:#,##0} organizations", sqlOrganizations.Count);

        await RemoveExistingOrganizations(sqlOrganizations);
        logger.LogInformation("Removed existing, now {Count:#,##0} organizations", sqlOrganizations.Count);

        /*** ADDING PLACES ***/
        if (sqlOrganizations.Count > 0)
        {
            var sqlOrganizationsCount = await AddPlaces(sqlOrganizations, sqlAddresses);
            logger.LogInformation("Added {Count:#,##0} places", sqlOrganizationsCount);
        }

        /*** COACHES ***/
        var sqlCoaches = (await oldSystemService.GetCoaches()).ToList();
        logger.LogInformation("Found {Count:#,##0} coaches", sqlCoaches.Count);

        await RemoveExistingCoaches(sqlCoaches);
        logger.LogInformation("Removed existing, now {Count:#,##0} coaches", sqlCoaches.Count);

        /*** ADDING PERSONS ***/
        if (sqlCoaches.Count > 0)
        {
            var (added, dupes) = await AddPersons(sqlCoaches);
            logger.LogInformation("Added {Count:#,##0} persons; {Dupes:#,##0} were dupes", added, dupes);
        }

        /*** JOINING PERSONS WITH PLACES ***/
        await JoinPersonsWithPlaces();


        /*** APPLICATIONS ***/
        var sqlApplications = (await oldSystemService.GetApplications()).ToList();
        logger.LogInformation("Found {Count:#,##0} applications", sqlApplications.Count);

        await RemoveExistingRequests(sqlApplications);
        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlApplications.Count);

        /*** ADDING REQUESTS ***/
        if (sqlApplications.Count > 0)
        {
            var sqlApplicationsCount = await AddRequests(sqlApplications);
            logger.LogInformation("Added {Count:#,##0} requests", sqlApplicationsCount);
        }

#if false
        /*** APPLICATION CLUBS ***/
        var sqlApplicationClubs = (await oldSystemService.GetApplicationClubs()).ToList();
        logger.LogInformation("Found {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        await RemoveExistingApplicationClubs(sqlApplicationClubs);
        logger.LogInformation("Removed existing, now {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        /*** ADDING REQUESTED CLUBS ***/
        if (sqlApplicationClubs.Count > 0)
        {
            var requestedClubsAdded = await AddRequestedClubs(sqlApplicationClubs);
            logger.LogInformation("Added {Count:#,##0} request clubs", requestedClubsAdded);
        }

        /*** JOIN REQUESTS TO PERSONS & PLACES ***/
        var personsLinked = await JoinRequestsToPersons(duplicateLookup);
        logger.LogInformation("{Count:#,##0} requests updated with person.", personsLinked);

        /*** JOIN REQUESTS TO PLACES ***/
        var placesLinked = await JoinRequestsToPlaces();
        logger.LogInformation("{Count:#,##0} requests updated with place.", placesLinked);

        /*** CLUBS ***/
        var sqlClubs = (await oldSystemService.GetClubs()).ToList();
        logger.LogInformation("Found {Count:#,##0} clubs", sqlClubs.Count);

        await RemoveExistingClubs(sqlClubs);
        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlClubs.Count);

        var clubsAdded = await AddClubs(sqlClubs, duplicateLookup);
        logger.LogInformation("Added {Count:#,##0} clubs", clubsAdded);

        /*** SKUS ***/
        var sqlSkus = (await oldSystemService.GetSkus()).ToList();
        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        await RemoveExistingSkus(sqlSkus);
        logger.LogInformation("Removed existing, now {Count:#,##0} skus", sqlSkus.Count);

        /*** ADDING SKUS ***/
        if (sqlSkus.Count > 0)
        {
            var skusAdded = await AddSkus(sqlSkus);
            logger.LogInformation("Added {Count:#,##0} skus", skusAdded);
        }

        /*** ORDERS ***/
        var sqlOrders = (await oldSystemService.GetOrders()).ToList();
        var sqlOrderTrackers = (await oldSystemService.GetOrderTrackers()).ToList();

        logger.LogInformation("Found {Orders:#,##0} orders with {Trackers:#,##0} trackers",
            sqlOrders.Count, sqlOrderTrackers.Count);

        await RemoveExistingOrders(sqlOrders);
        logger.LogInformation("Removed existing, now {Count:#,##0} orders", sqlOrders.Count);

        if (sqlOrders.Count > 0)
        {
            var ordersAdded = await AddOrders(sqlOrders, sqlAddresses, sqlOrderTrackers);
            logger.LogInformation("Added {Count:#,##0} orders", ordersAdded);
        }

        /*** ORDER SKUS ***/
        var sqlOrderSkus = (await oldSystemService.GetOrderSkus()).ToList();
        logger.LogInformation("Found {Count:#,##0} order skus", sqlOrderSkus.Count);

        await RemoveExistingOrderSkus(sqlOrderSkus);
        logger.LogInformation("Removed existing, now {Count:#,##0} order skus", sqlOrderSkus.Count);

        /*** JOIN ORDERS & SKUS TO ORDER SKUS ***/
        var (addedOrderSkus, skippedOrderSkus) = await JoinOrderSkus(sqlOrderSkus);
        logger.LogInformation("Added {Count:#,##0} orderSkus; skipped {Skipped:#,##0}", addedOrderSkus, skippedOrderSkus); 
#endif

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }

    #region Add Entities
    private async Task<(int, int)> AddPersons(List<CoachSql> sqlCoaches)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var added = 0;
        var dupes = 0;
        var toRemove = new List<CoachSql>();

        ConsoleEx.StartProgress("Adding persons: ");
        for (int index = 0; index < sqlCoaches.Count; index++)
        {
            var sqlCoach = sqlCoaches[index];

            // check for duplicates
            var person = dbContext.Persons
                .FirstOrDefault(p =>
                    !String.IsNullOrEmpty(p.Email) &&
                    p.Email.Trim().ToLower() == sqlCoach.Email.Trim().ToLower());

            // create the person if new
            if (person == null)
            {
                person = new PersonDb()
                {
                    FirstName = sqlCoach.FirstName,
                    LastName = sqlCoach.LastName,
                    Email = sqlCoach.Email,
                    TimeZone = sqlCoach.TimeZone,
                    Phone = sqlCoach.Phone +
                            (String.IsNullOrEmpty(sqlCoach.PhoneExt) ? null : $" x{sqlCoach.PhoneExt}"),
                    Notes = new List<PersonNoteDb>(),
                    CreatedOn = sqlCoach.CreatedOn
                };

                dbContext.Persons.Add(person);
                added++;

                await dbContext.SaveChangesAsync();
            }

            // otherwise, we'll be removing this person from the list of coaches
            else
            {
                toRemove.Add(sqlCoach);
                dupes++;
            }

            // either way, add any notes we might have
            if (!String.IsNullOrEmpty(sqlCoach.Notes))
            {
                person.Notes ??= new List<PersonNoteDb>();
                person.Notes.Add(new()
                {
                    Author = SoftCrowConstants.Display.System,
                    Content = sqlCoach.Notes
                });
            }

            // and create the connections

            //OldSystemCoachId = sqlCoach.OldSystemCoachId,
            if (sqlCoach.OldSystemCoachId != null)
            {
                var oldNewCoach = new OldNewDb()
                {
                    OldTableName = CoachTable,
                    OldId = sqlCoach.OldSystemCoachId!.Value,
                    NewTableName = nameof(PersonDb),
                    NewId = person.PersonId
                };
                dbContext.OldNews.Add(oldNewCoach);
            }

            //OldSystemOrganizationId = sqlCoach.OldSystemOrganizationId,
            if (sqlCoach.OldSystemOrganizationId != null)
            {
                var oldNewOrganization = new OldNewDb()
                {
                    OldTableName = OrganizationTable,
                    OldId = sqlCoach.OldSystemOrganizationId!.Value,
                    NewTableName = nameof(PersonDb),
                    NewId = person.PersonId
                };
                dbContext.OldNews.Add(oldNewOrganization);
            }

            //OldSystemUserId = sqlCoach.OldSystemUserId,
            if (sqlCoach.OldSystemUserId != null)
            {
                var oldNewUser = new OldNewDb()
                {
                    OldTableName = UserTable,
                    OldId = sqlCoach.OldSystemUserId!.Value,
                    NewTableName = nameof(PersonDb),
                    NewId = person.PersonId
                };
                dbContext.OldNews.Add(oldNewUser);
            }

            //OldSystemCompanyId = sqlCoach.OldSystemCompanyId,
            if (sqlCoach.OldSystemCompanyId != null)
            {
                var oldNewCompany = new OldNewDb()
                {
                    OldTableName = CompanyTable,
                    OldId = sqlCoach.OldSystemCompanyId!.Value,
                    NewTableName = nameof(PersonDb),
                    NewId = person.PersonId
                };
                dbContext.OldNews.Add(oldNewCompany);
            }

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlCoaches.Count);
        }
        ConsoleEx.EndProgress();

        // remove duplicates from the list
        sqlCoaches.RemoveAll(c => toRemove.Contains(c));

        return (added, dupes);
    }

    private async Task<int> AddPlaces(List<OrganizationSql> sqlOrganizations, List<AddressSql> sqlAddresses)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var sqlOrganizationsCount = sqlOrganizations.Count;
        ConsoleEx.StartProgress("Adding places: ");
        var added = 0;
        for (int index = 0; index < sqlOrganizationsCount; index++)
        {
            var sqlOrganization = sqlOrganizations[index];
            var sqlAddress = sqlAddresses
                                 .FirstOrDefault(a => a.OldSystemUsaPostalId == sqlOrganization.OldSystemPostalAddressId) ??
                             throw new Exception($"Could not find address ({sqlOrganization.OldSystemPostalAddressId}) for organization ({sqlOrganization.OldSystemOrganizationId})");

            var place = new PlaceDb()
            {
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
                place.Notes = [
                    new()
                    {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlOrganization.Notes
                    }
                ];
            dbContext.Places.Add(place);
            await dbContext.SaveChangesAsync();
            added++;

            //OldSystemCompanyId = sqlOrganization.OldSystemCompanyId,
            if (sqlOrganization.OldSystemOrganizationId != null)
            {
                var oldNewCompany = new OldNewDb()
                {
                    OldTableName = CompanyTable,
                    OldId = sqlOrganization.OldSystemCompanyId!.Value,
                    NewTableName = nameof(PlaceDb),
                    NewId = place.PlaceId
                };
                dbContext.OldNews.Add(oldNewCompany);
            }

            //OldSystemOrganizationId = sqlOrganization.OldSystemOrganizationId,
            if (sqlOrganization.OldSystemOrganizationId != null)
            {
                var oldNewOrganization = new OldNewDb()
                {
                    OldTableName = OrganizationTable,
                    OldId = sqlOrganization.OldSystemOrganizationId!.Value,
                    NewTableName = nameof(PlaceDb),
                    NewId = place.PlaceId
                };
                dbContext.OldNews.Add(oldNewOrganization);
            }

            //OldSystemPostalAddressId = sqlOrganization.OldSystemPostalAddressId,
            if (sqlOrganization.OldSystemPostalAddressId != null)
            {
                var oldNewAddress = new OldNewDb()
                {
                    OldTableName = PostalAddressTable,
                    OldId = sqlOrganization.OldSystemPostalAddressId!.Value,
                    NewTableName = nameof(PlaceDb),
                    NewId = place.PlaceId
                };
                dbContext.OldNews.Add(oldNewAddress);
            }

            //OldSystemUsaPostalId = sqlAddress.OldSystemUsaPostalId,
            if (sqlAddress.OldSystemUsaPostalId != null)
            {
                var oldNewUsaPostal = new OldNewDb()
                {
                    OldTableName = UsaPostalTable,
                    OldId = sqlAddress.OldSystemUsaPostalId!.Value,
                    NewTableName = nameof(PlaceDb),
                    NewId = place.PlaceId
                };
                dbContext.OldNews.Add(oldNewUsaPostal);
            }
            
            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlOrganizationsCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }
    #endregion

    #region Joins
    private async Task JoinPersonsWithPlaces()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var oldNewCoaches = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(PersonDb) && o.OldTableName == OrganizationTable)
            .ToListAsync();

        var personsWithPlaceIds = oldNewCoaches.Select(o => o.NewId).ToList();
        var personsMissingPlace = await dbContext.Persons
            .Where(p => p.PlaceId == null && personsWithPlaceIds.Contains(p.PersonId))
            .ToListAsync();

        var personsMissingPlaceCount = personsMissingPlace.Count;
        logger.LogInformation("Found {Count:#,##0} persons without a place", personsMissingPlaceCount);

        var updated = 0;
        var skipped = 0;
        if (personsMissingPlaceCount > 0)
        {
            var oldNewOrganizations = await dbContext.OldNews
                .Where(o => o.NewTableName == nameof(PlaceDb) && o.OldTableName == OrganizationTable)
                .ToListAsync();

            ConsoleEx.StartProgress("Joining persons with places: ");
            for (int index = 0; index < personsMissingPlaceCount; index++)
            {
                var person = personsMissingPlace[index];
                var organizationId = oldNewCoaches.FirstOrDefault(c => c.NewId == person.PersonId)?.OldId ??
                                throw new UnreachableException("Could not match person in OldNew list");
                var placeId = oldNewOrganizations.FirstOrDefault(o => o.OldId == organizationId)?.NewId;
                if (placeId == null) { skipped++; continue; }

                person.PlaceId = placeId;
                updated++;

                if ((index + 1) % SaveBlock == 0)
                    await dbContext.SaveChangesAsync();
                ConsoleEx.ShowProgress((float)index / (float)personsMissingPlaceCount);
            }
            await dbContext.SaveChangesAsync();
            ConsoleEx.EndProgress();

            logger.LogInformation("{Count:#,##0} persons updated with a place; {Skipped:#,##0} skipped.", updated, skipped);
        }
    }

    #endregion

    #region Remove Dupes
    private async Task RemoveExistingOrganizations(List<OrganizationSql> sqlOrganizations)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(PlaceDb))
            .Select(o => o.OldId)
            .ToListAsync();
        sqlOrganizations.RemoveAll(m => existingGuids.Contains(m.OldSystemOrganizationId!.Value));
    }

    private async Task RemoveExistingCoaches(List<CoachSql> sqlCoaches)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(PersonDb))
            .Select(o => o.OldId)
            .ToListAsync();
        sqlCoaches.RemoveAll(m => existingGuids.Contains(m.OldSystemCoachId!.Value));
    }
    #endregion

#if false

    private static PersonDb? GetPersonWithLookup(List<PersonDb> allPersons, Dictionary<Guid, Guid> lookup, Guid? toMatch)
    {
        if (toMatch == null) return null;

        var person = allPersons.FirstOrDefault(a => a.OldSystemCoachId == toMatch);
        if (person == null && lookup.TryGetValue(toMatch.Value, out var lookupId))
            person = allPersons.FirstOrDefault(a => a.OldSystemCoachId == lookupId);

        return person;
    }

    private async Task<(int addedOrderSkus, int skippedOrderSkus)> JoinOrderSkus(List<OrderSkuSql> sqlOrderSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var allOrders = dbContext.Orders.ToList();
        var allSkus = dbContext.Offers.ToList();

        var sqlOrderSkusCount = sqlOrderSkus.Count;
        ConsoleEx.StartProgress("Joining orders & skus with order skus: ");
        var addedOrderSkus = 0;
        var skippedOrderSkus = 0;
        for (int index = 0; index < sqlOrderSkusCount; index++)
        {
            var sqlOrderSku = sqlOrderSkus[index];

            var order = allOrders.FirstOrDefault(o => o.OldSystemOrderId == sqlOrderSku.OldSystemOrderId);
            if (order == null) { skippedOrderSkus++; continue; }

            var sku = allSkus.FirstOrDefault(o => o.OldSystemSkuId == sqlOrderSku.OldSystemSkuId);
            if (sku == null) { skippedOrderSkus++; continue; }

            var orderSku = new OrderClubDb()
            {
                OldSystemOrderSkuId = sqlOrderSku.OldSystemOrderSkuId,
                OldSystemOrderId = sqlOrderSku.OldSystemOrderId,
                OldSystemSkuId = sqlOrderSku.OldSystemSkuId,
                Ordinal = sqlOrderSku.Ordinal,
                Quantity = sqlOrderSku.Quantity,
                Order = order,
                Offer = sku
            };
            dbContext.OrderSkus.Add(orderSku);
            addedOrderSkus++;

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlOrderSkusCount);
        }
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return (addedOrderSkus, skippedOrderSkus);
    }

    private async Task RemoveExistingOrderSkus(List<OrderSkuSql> sqlOrderSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingOrderSkuIds = await dbContext.OrderSkus.Select(o => o.OldSystemOrderSkuId).ToListAsync();
        sqlOrderSkus.RemoveAll(m => existingOrderSkuIds.Contains(m.OldSystemOrderSkuId));
    }

    private async Task<int> AddOrders(List<OrderSql> sqlOrders, List<AddressSql> sqlAddresses, List<OrderTrackerSql> sqlOrderTrackers)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var allClubs = await dbContext.Clubs.ToListAsync();

        var sqlOrdersCount = sqlOrders.Count;
        ConsoleEx.StartProgress("Adding orders & shipments, plus persons & places to orders: ");
        var ordersAdded = 0;
        for (int index = 0; index < sqlOrdersCount; index++)
        {
            var sqlOrder = sqlOrders[index];

            // start by finding the original club & address
            var orderClub = allClubs.FirstOrDefault(c => c.OldSystemClubId == sqlOrder.OldSystemClubId);
            var sqlAddress = sqlAddresses.FirstOrDefault(a => a.OldSystemUsaPostalId == sqlOrder.OldSystemShippingAddressId) ??
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

        return ordersAdded;
    }

    private async Task RemoveExistingOrders(List<OrderSql> sqlOrders)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingOrderIds = await dbContext.Orders.Select(o => o.OldSystemOrderId).ToListAsync();
        sqlOrders.RemoveAll(m => existingOrderIds.Contains(m.OldSystemOrderId));
    }

    private async Task<int> AddSkus(List<SkuSql> sqlSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var skusCount = sqlSkus.Count;
        var skusAdded = 0;
        ConsoleEx.StartProgress("Adding skus: ");
        for (int index = 0; index < skusCount; index++)
        {
            var sqlSku = sqlSkus[index];

            var sku = new OfferDb()
            {
                OldSystemSkuId = sqlSku.OldSystemSkuId,
                FulcoId = sqlSku.Key,
                Title = sqlSku.Name,
                Status = sqlSku.Status ?? OfferStatus.Inactive,
                Year = GetYearFromSqlSku(sqlSku),
                Season = sqlSku.Season!.Value,
                AgeLevel = sqlSku.AgeLevel!.Value,
                Version = GetVersionFromSqlSku(sqlSku),
                Description = sqlSku.Notes
            };
            dbContext.Offers.Add(sku);

            skusAdded++;

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)skusCount);
        }
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return skusAdded;
    }

    private async Task RemoveExistingSkus(List<SkuSql> sqlSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingSkuIds = await dbContext.Offers.Select(o => o.OldSystemSkuId).ToListAsync();
        sqlSkus.RemoveAll(m => existingSkuIds.Contains(m.OldSystemSkuId));
    }

    private async Task<int> AddClubs(List<ClubSql> sqlClubs, Dictionary<Guid, Guid> duplicateLookup)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var allPersons = await dbContext.Persons.ToListAsync();
        var allPlaces = await dbContext.Places.ToListAsync();

        ConsoleEx.StartProgress("Adding clubs, plus persons & places to clubs: ");

        var sqlClubsCount = sqlClubs.Count;
        int clubsAdded = 0;
        for (int index = 0; index < sqlClubsCount; index++)
        {
            var sqlClub = sqlClubs[index];

            // start by finding the original coach
            var foundPerson = GetPersonWithLookup(allPersons, duplicateLookup, sqlClub.OldSystemCoachId);
            if (foundPerson == null && sqlClub.OldSystemCoachId != null)
            {
                var deletedPerson = (await oldSystemService.GetDeletedCoach(sqlClub.OldSystemCoachId.Value));
                if (deletedPerson != null)
                    foundPerson = allPersons.FirstOrDefault(c => c.Email == deletedPerson.Email);
            }
            var person = foundPerson ??
                         throw new Exception($"Could not find person ({sqlClub.OldSystemCoachId}) for club ({sqlClub.OldSystemClubId})");

            // then by finding the original place
            var place = allPlaces.FirstOrDefault(c => c.OldSystemOrganizationId == sqlClub.OldSystemOrganizationId) ??
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
                Season = sqlClub.Season!.Value,
                AgeLevel = sqlClub.AgeLevel!.Value,
                StartsOn = sqlClub.StartsOn,
                ClubPersons = []
            };
            if (!String.IsNullOrEmpty(sqlClub.Notes))
                club.Notes = (List<ClubNoteDb>)
                [
                    new()
                    {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlClub.Notes
                    }
                ];
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
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return clubsAdded;
    }

    private async Task RemoveExistingClubs(List<ClubSql> sqlClubs)
    {
        await using (var dbContext = await dbContextFactory.CreateDbContextAsync())
        {
            var existingClubIds = await dbContext.Clubs.Select(o => o.OldSystemClubId).ToListAsync();
            sqlClubs.RemoveAll(m => existingClubIds.Contains(m.OldSystemClubId));
        }
    }

    private async Task<int> JoinRequestsToPlaces()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return placesLinked;
    }

    private async Task<int> JoinRequestsToPersons(Dictionary<Guid, Guid> duplicateLookup)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var allPersons = await dbContext.Persons.ToListAsync();

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
                var person = GetPersonWithLookup(allPersons, duplicateLookup, oldCoachLink.Value);
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
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return personsLinked;
    }

    private async Task<int> AddRequestedClubs(List<ApplicationClubSql> sqlApplicationClubs)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var allRequests = await dbContext.Requests.ToListAsync();

        var applicationClubsCount = sqlApplicationClubs.Count;
        ConsoleEx.StartProgress("Adding requested clubs: ");
        var requestedClubsAdded = 0;
        for (int index = 0; index < applicationClubsCount; index++)
        {
            var appClub = sqlApplicationClubs[index];
            var request = allRequests.FirstOrDefault(r =>
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

        return requestedClubsAdded;
    }

    private async Task RemoveExistingApplicationClubs(List<ApplicationClubSql> sqlApplicationClubs)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingApplicationClubIds = await dbContext.RequestedClubs
            .Select(o => o.OldSystemApplicationClubId)
            .ToListAsync();
        sqlApplicationClubs.RemoveAll(m => existingApplicationClubIds.Contains(m.OldSystemApplicationClubId));
    }

    private async Task<int> AddRequests(List<ApplicationSql> sqlApplications)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
                              (String.IsNullOrEmpty(sqlApplication.ApplicantPhoneExt)
                                  ? null
                                  : $" x{sqlApplication.ApplicantPhoneExt}"),
                PlaceName = sqlApplication.OrganizationName,
                PlaceType = sqlApplication.OrganizationType,
                PlaceTypeOther = sqlApplication.OrganizationTypeOther,
                PlaceTaxIdentifier = sqlApplication.OrganizationTaxIdentifier,
                WorkshopCode = sqlApplication.WorkshopCode,
                ReferenceSource = null,
                ReferenceSourceOther = null,
                AppointmentId = sqlApplication.AppointmentId,
                Comments = sqlApplication.Comments,
                SubmittedOn = sqlApplication.SubmittedOn ?? throw new Exception("Missing Submitted On"),
                CreatedOn = sqlApplication.CreatedOn
            };
            if (!String.IsNullOrEmpty(sqlApplication.Notes))
                request.Notes = (List<RequestNoteDb>)
                [
                    new() {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlApplication.Notes }
                ];
            dbContext.Requests.Add(request);

            if ((index + 1) % SaveBlock == 0)
                await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
        }
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return sqlApplicationsCount;
    }

    private async Task RemoveExistingRequests(List<ApplicationSql> sqlApplications)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingRequestIds = await dbContext.Requests.Select(o => o.OldSystemApplicationId).ToListAsync();
        sqlApplications.RemoveAll(m => existingRequestIds.Contains(m.OldSystemApplicationId));
    }

#endif

    private readonly Regex _parseSkuKey =
        new Regex(@"^C8\.S\d.(?<year>F[\d+]+)(?<alt>ALT)?\-G(?:K2|35)(?<extra>.*)$", RegexOptions.Compiled | RegexOptions.Singleline);
    private string GetYearFromSqlSku(SkuSql sqlSku)
    {
        var yearString = GetYearFromKey(sqlSku.Key);
        return yearString ?? $"X{sqlSku.CreatedOn:yy}";
    }
    private string? GetVersionFromSqlSku(SkuSql sqlSku)
    {
        var extra = GetExtraFromKey(sqlSku.Key);
        if (String.IsNullOrWhiteSpace(extra)) extra = null;
        return (extra == "R") ? null : extra;
    }

    private string? GetYearFromKey(string key)
    {
        var match = _parseSkuKey.Match(key);
        return (match.Success) ? match.Groups["year"].Value : null;
    }
    private string? GetExtraFromKey(string key)
    {
        var match = _parseSkuKey.Match(key);
        var extra = (match.Success) ? match.Groups["extra"].Value : null;
        if (match.Success && !String.IsNullOrWhiteSpace(match.Groups["alt"].Value)) extra += "ALT";
        return extra?.Trim();
    }
}