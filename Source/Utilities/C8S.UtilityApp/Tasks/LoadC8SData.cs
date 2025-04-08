using C8S.Domain.EFCore.Contexts;
using C8S.UtilityApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class LoadC8SData(
    ILogger<LoadC8SData> logger,
    LoadC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    OldSystemService oldSystemService)
    : BaseC8SData(dbContextFactory, oldSystemService)
{
    public override async Task<int> Launch()
    {
        logger.LogInformation("=== {Name} ===", nameof(LoadC8SData));

        if (!Directory.Exists(options.InputPath))
            throw new Exception($"Missing input path: {options.InputPath}");

        // check the connection quickly
        // ReSharper disable once UseAwaitUsing
        // ReSharper disable once MethodHasAsyncOverload
        using (var tempDb = DbContextFactory.CreateDbContext())
        {
            var sqlConnection = tempDb.Database.GetConnectionString();
            logger.LogInformation("Connection: {Connection}", sqlConnection);
        }
        logger.LogInformation("OldSystem: {Connection}", OldSystemService.ConnectionString);

        Console.Write($"");

        logger.LogInformation("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;
        Console.WriteLine();

        /*** VARIABLES USED THROUGHOUT ***/
        ClubLookup? clubIdLookup = null;
        PersonLookup? personIdLookup = null;
        PlaceLookup? placeIdLookup = null;
        TicketLookup? ticketIdLookup = null;
        OfferLookup? offerIdLookup = null;
        OrderLookup? orderIdLookup = null;
        KitLookup? kitIdLookup = null;

        /*** SET UP OFFERS & KITS ***/
        var sqlSkus = (await OldSystemService.GetSkus()).ToList();
        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        await RemoveExistingSkus(sqlSkus);
        logger.LogInformation("Removed existing, now {Count:#,##0} skus", sqlSkus.Count);

        if (sqlSkus.Count > 0)
        {
            var offersAdded = await CreateOffersFromSkus(sqlSkus);
            logger.LogInformation("Added {Count:#,##0} offers.", offersAdded);

            offerIdLookup ??= await CreateOffersLookup();

            var kitsAdded = await CreateKitsFromSkus(sqlSkus, offerIdLookup);
            logger.LogInformation("Added {Count:#,##0} kits.", kitsAdded);
        }

        /*** ADDRESSES ***/
        var sqlAddresses = (await OldSystemService.GetAddresses()).ToList();
        logger.LogInformation("Found {Count:#,##0} addresses", sqlAddresses.Count);

        /*** ORGANIZATIONS ***/
        var sqlOrganizations = (await OldSystemService.GetOrganizations()).ToList();
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
        var sqlCoaches = (await OldSystemService.GetCoaches()).ToList();
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
        logger.LogInformation("Looking for persons without a place");
        var (joinedPersons, skippedPersons) = await JoinPersonsWithPlaces();
        logger.LogInformation("{Joined:#,##0} persons joined with a place; {Skipped:#,##0} skipped.", joinedPersons, skippedPersons);


        /*** APPLICATIONS ***/
        var sqlApplications = (await OldSystemService.GetApplications()).ToList();
        logger.LogInformation("Found {Count:#,##0} applications", sqlApplications.Count);

        await RemoveExistingTickets(sqlApplications);
        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlApplications.Count);

        /*** ADDING TICKETS ***/
        if (sqlApplications.Count > 0)
        {
            var sqlApplicationClubs = (await OldSystemService.GetApplicationClubs()).ToList();
            logger.LogInformation("Found {Count:#,##0} application clubs", sqlApplicationClubs.Count);

            var sqlApplicationsCount = await AddTickets(sqlApplications, sqlApplicationClubs);
            logger.LogInformation("Added {Count:#,##0} tickets + requests", sqlApplicationsCount);

            personIdLookup ??= await CreatePersonsLookup();
            placeIdLookup ??= await CreatePlacesLookup();
            ticketIdLookup ??= await CreateTicketsLookup();

            /*** JOINING PERSONS TO TICKETS ***/
            var (personsAdded, personsExisting, personsMatched) = await JoinPersonsToTickets(sqlApplications, ticketIdLookup, personIdLookup);
            var personsJoined = personsAdded + personsExisting + personsMatched;
            logger.LogInformation("Joined {Joined:#,##0} persons to tickets; {Added:#,##0} added, {Existing:#,##0} existing, {Matched:#,##0} matched.",
                personsJoined, personsAdded, personsExisting, personsMatched);

            /*** JOINING PLACES TO TICKETS ***/
            var (placesAdded, placesExisting, placesSkipped) = await JoinPlacesToTickets(sqlApplications, sqlAddresses, ticketIdLookup, placeIdLookup);
            var placesJoined = placesAdded + placesExisting + placesSkipped;
            logger.LogInformation("Joined {Joined:#,##0} places to tickets; {Added:#,##0} added, {Existing:#,##0} existing, {Skipped:#,##0} skipped.",
                placesJoined, placesAdded, placesExisting, placesSkipped);
        }

        /*** ADDING ORDERS ***/
        var sqlOrders = (await OldSystemService.GetOrders()).ToList();
        var sqlOrderTrackers = (await OldSystemService.GetOrderTrackers()).ToList();

        logger.LogInformation("Found {Orders:#,##0} orders with {Trackers:#,##0} trackers",
            sqlOrders.Count, sqlOrderTrackers.Count);

        await RemoveExistingOrders(sqlOrders);
        logger.LogInformation("Removed existing, now {Count:#,##0} orders", sqlOrders.Count);

        if (sqlOrders.Count > 0)
        {
            var ordersAdded = await AddOrders(sqlOrders, sqlOrderTrackers, sqlAddresses);
            logger.LogInformation("Added {Count:#,##0} orders", ordersAdded);
        }

        /*** ORDER SKUS ***/
        var sqlOrderSkus = (await OldSystemService.GetOrderSkus()).ToList();
        logger.LogInformation("Found {Count:#,##0} order skus", sqlOrderSkus.Count);

        await RemoveExistingOrderSkus(sqlOrderSkus);
        logger.LogInformation("Removed existing, now {Count:#,##0} order skus", sqlOrderSkus.Count);

        if (sqlOrderSkus.Count > 0)
        {
            orderIdLookup ??= await CreateOrdersLookup();
            offerIdLookup ??= await CreateOffersLookup();

            var offersJoined = await JoinOffersToOrders(sqlOrderSkus, orderIdLookup, offerIdLookup);
            logger.LogInformation("Joined {Count:#,##0} offers to orders.", offersJoined);
        }

        /*** CLUBS ***/
        var sqlClubs = (await OldSystemService.GetClubs()).ToList();
        logger.LogInformation("Found {Count:#,##0} total clubs", sqlClubs.Count);

        await RemoveExistingClubs(sqlClubs);
        logger.LogInformation("Removed existing, now {Count:#,##0} total clubs", sqlClubs.Count);

        if (sqlClubs.Count > 0)
        {
            orderIdLookup ??= await CreateOrdersLookup();
            placeIdLookup ??= await CreatePlacesLookup();
            kitIdLookup ??= await CreateKitsLookup();

            var (clubsAdded, clubsMismatched) = await AddClubs(sqlClubs, orderIdLookup, placeIdLookup, kitIdLookup);
            logger.LogInformation("Added {Added:#,##0} clubs; {Mismatched:#,##0} mismatched.", clubsAdded, clubsMismatched);

            clubIdLookup ??= await CreateClubsLookup();
            personIdLookup ??= await CreatePersonsLookup();

            var (joined, skipped) = await JoinPersonsToClubs(sqlClubs, clubIdLookup, personIdLookup);
            logger.LogInformation("Joined {Joined:#,##0} persons to clubs; {Skipped:#,##0} clubs skipped.", joined, skipped);
        }

        /*** KIT PAGES ***/
        var (kitPagesAdded, kitPagesSkipped) = await AddKitPages();
        logger.LogInformation("Added {Added:#,##0} kit pages; {Skipped:#,##0} skipped.", kitPagesAdded, kitPagesSkipped);

        /*** PERMISSIONS ***/
        var personIds = await GetPersonIdsForPermissions();
        logger.LogInformation("Found {Count:#,##0} persons for permissions", personIds.Count);

        if (personIds.Count > 0)
        {
            var (permissionsAdded, permissionsSkipped) = await AddPermissions(personIds);
            logger.LogInformation("Added {Added:#,##0} permissions; {Skipped:#,##0} skipped.", permissionsAdded, permissionsSkipped);
        } 

        logger.LogInformation("{Name}: complete.", nameof(LoadC8SData));
        return 0;
    }
}