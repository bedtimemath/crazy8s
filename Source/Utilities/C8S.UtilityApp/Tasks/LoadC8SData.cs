using System.Diagnostics;
using System.Text.RegularExpressions;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using C8S.UtilityApp.Services;
using C8S.WordPress.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SC.Common;
using SC.Common.Extensions;

namespace C8S.UtilityApp.Tasks;

internal class LoadC8SData(
    ILogger<LoadC8SData> logger,
    LoadC8SDataOptions options,
    IDbContextFactory<C8SDbContext> dbContextFactory,
    OldSystemService oldSystemService)
    : IActionLauncher
{
    private const string SkuTable = "Sku";
    private const string ClubTable = "Club";
    private const string OrderTable = "Order";
    private const string OrderSkuTable = "OrderSku";
    private const string ApplicationTable = "Application";
    private const string ApplicationClubTable = "ApplicationClub";
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

        /*** VARIABLES USED THROUGHOUT ***/
        ClubLookup? clubIdLookup = null;
        PersonLookup? personIdLookup = null;
        PlaceLookup? placeIdLookup = null;
        TicketLookup? ticketIdLookup = null;
        OfferLookup? offerIdLookup = null;
        OrderLookup? orderIdLookup = null;
        KitLookup? kitIdLookup = null;

        /*** SET UP OFFERS & KITS ***/
        var sqlSkus = (await oldSystemService.GetSkus()).ToList();
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

        await RemoveExistingTickets(sqlApplications);
        logger.LogInformation("Removed existing, now {Count:#,##0} applications", sqlApplications.Count);

        /*** ADDING TICKETS ***/
        if (sqlApplications.Count > 0)
        {
            var sqlApplicationClubs = (await oldSystemService.GetApplicationClubs()).ToList();
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
        var sqlOrders = (await oldSystemService.GetOrders()).ToList();
        var sqlOrderTrackers = (await oldSystemService.GetOrderTrackers()).ToList();

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
        var sqlOrderSkus = (await oldSystemService.GetOrderSkus()).ToList();
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
        var sqlClubs = (await oldSystemService.GetClubs()).ToList();
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

    #region Get Entities
    private async Task<List<int>> GetPersonIdsForPermissions()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var persons = await dbContext.Persons
            .AsNoTracking()
            .AsSingleQuery()
            .Include(p => p.ClubPersons)
            .ThenInclude(cp => cp.Club)
            .ThenInclude(c => c.Kit)
            .Where(p => p.ClubPersons.Any(cp => cp.Club.Kit.KitPage != null))
            .Select(p => p.PersonId)
            .ToListAsync();

        return persons;
    }
    #endregion

    #region Add Entities
    private async Task<int> CreateOffersFromSkus(List<SkuSql> sqlSkus)
    {
        var existingFulcoIds = new List<string>();

        var skusCount = sqlSkus.Count;
        var added = 0;
        ConsoleEx.StartProgress("Adding offers: ");
        for (int index = 0; index < skusCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var sqlSku = sqlSkus[index];

            var year = GetYearFromSqlSku(sqlSku);
            var season = sqlSku.Season ?? throw new Exception("Missing Season");
            var version = GetVersionFromSqlSku(sqlSku);

            var offer = dbContext.Offers
                .FirstOrDefault(o => o.Year == year && o.Season == season && o.Version == version);
            if (offer == null)
            {
                var fulcoId = GetFulcoIdFromSqlSky(sqlSku);
                while (existingFulcoIds.Contains(fulcoId))
                    fulcoId = fulcoId.IncrementFinalDigits();
                existingFulcoIds.Add(fulcoId);

                offer = new OfferDb()
                {
                    FulcoId = fulcoId,
                    Title = DropGradesFromTitle(sqlSku.Name),
                    Status = sqlSku.Status ?? OfferStatus.Inactive,
                    Year = year,
                    Season = season,
                    Version = version,
                    Description = sqlSku.Notes,
                    CreatedOn = sqlSku.CreatedOn
                };
                dbContext.Offers.Add(offer);

                await dbContext.SaveChangesAsync();
                added++;
            }

            // remember that this has been handled
            if (sqlSku.OldSystemSkuId != null)
            {
                var oldNewOffer = new OldNewDb()
                {
                    OldTableName = SkuTable,
                    OldId = sqlSku.OldSystemSkuId!.Value,
                    NewTableName = nameof(OfferDb),
                    NewId = offer.OfferId
                };
                dbContext.OldNews.Add(oldNewOffer);

                await dbContext.SaveChangesAsync();
            }

            ConsoleEx.ShowProgress((float)index / (float)skusCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }

    private async Task<int> CreateKitsFromSkus(List<SkuSql> sqlSkus, OfferLookup offerIdLookup)
    {
        var skusCount = sqlSkus.Count;
        var added = 0;
        ConsoleEx.StartProgress("Adding kits: ");
        for (int index = 0; index < skusCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var sqlSku = sqlSkus[index];

            var year = GetYearFromSqlSku(sqlSku);
            var season = sqlSku.Season ?? throw new Exception("Missing Season");
            var ageLevel = sqlSku.AgeLevel ?? throw new Exception("Missing AgeLevel");
            var version = GetVersionFromSqlSku(sqlSku);

            var kit = await dbContext.Kits
                .FirstOrDefaultAsync(k => k.Year == year && k.Season == season && k.AgeLevel == ageLevel && k.Version == version);
            if (kit == null)
            {
                if (!offerIdLookup.TryGetValue(sqlSku.OldSystemSkuId ?? Guid.Empty, out var offerId))
                    throw new Exception($"Could not find offer for sku ({sqlSku.OldSystemSkuId})");
                var offer = await dbContext.Offers.FirstOrDefaultAsync(o => o.OfferId == offerId) ??
                            throw new Exception($"Could not find offer ({offerId})");

                kit = new KitDb()
                {
                    Status = sqlSku.Status switch
                    {
                        OfferStatus.Draft => KitStatus.Draft,
                        OfferStatus.Active => KitStatus.Active,
                        OfferStatus.Inactive => KitStatus.Inactive,
                        null => KitStatus.Inactive,
                        _ => throw new ArgumentOutOfRangeException(nameof(sqlSku.Status))
                    },
                    Offer = offer,
                    Year = year,
                    Season = season,
                    AgeLevel = ageLevel,
                    Version = version,
                    Comments = sqlSku.Notes,
                    CreatedOn = sqlSku.CreatedOn
                };
                dbContext.Kits.Add(kit);

                await dbContext.SaveChangesAsync();
                added++;
            }

            // remember that this has been handled
            if (sqlSku.OldSystemSkuId != null)
            {
                var oldNewKit = new OldNewDb()
                {
                    OldTableName = SkuTable,
                    OldId = sqlSku.OldSystemSkuId!.Value,
                    NewTableName = nameof(KitDb),
                    NewId = kit.KitId
                };
                dbContext.OldNews.Add(oldNewKit);

                await dbContext.SaveChangesAsync();
            }

            ConsoleEx.ShowProgress((float)index / (float)skusCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }

    private async Task<(int, int)> AddPersons(List<CoachSql> sqlCoaches)
    {
        var added = 0;
        var dupes = 0;
        var toRemove = new List<CoachSql>();

        ConsoleEx.StartProgress("Adding persons: ");
        for (int index = 0; index < sqlCoaches.Count; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var sqlCoach = sqlCoaches[index];

            // check for duplicates
            var person = dbContext.Persons
                .FirstOrDefault(p =>
                    !String.IsNullOrEmpty(p.Email) &&
                    p.Email.Trim().ToLower() == sqlCoach.Email.Trim().ToLower());

            // create the person if new
            if (person == null)
            {
                person = CreatePersonFromCoach(sqlCoach);

                dbContext.Persons.Add(person);
                added++;

                await dbContext.SaveChangesAsync();

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
            }

            // otherwise, we'll be removing this person from the list of coaches
            else
            {
                toRemove.Add(sqlCoach);
                dupes++;
            }

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

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlCoaches.Count);
        }
        ConsoleEx.EndProgress();

        // remove duplicates from the list
        sqlCoaches.RemoveAll(c => toRemove.Contains(c));

        return (added, dupes);
    }

    private PersonDb CreatePersonFromCoach(CoachSql sqlCoach) => new ()
        {
            FirstName = sqlCoach.FirstName,
            LastName = sqlCoach.LastName,
            Email = sqlCoach.Email,
            TimeZone = sqlCoach.TimeZone,
            Phone = sqlCoach.Phone +
                    (String.IsNullOrEmpty(sqlCoach.PhoneExt) ? null : $" x{sqlCoach.PhoneExt}"),
            JobTitle = JobTitleFromRole(sqlCoach.Role),
            JobTitleOther = JobTitleOtherFromRole(sqlCoach.Role),
            Notes = new List<PersonNoteDb>(),
            CreatedOn = sqlCoach.CreatedOn
        };

    private async Task<int> AddPlaces(List<OrganizationSql> sqlOrganizations, List<AddressSql> sqlAddresses)
    {
        var sqlOrganizationsCount = sqlOrganizations.Count;
        ConsoleEx.StartProgress("Adding places: ");
        var added = 0;
        for (int index = 0; index < sqlOrganizationsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

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
                CreatedOn = sqlOrganization.CreatedOn
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

    private async Task<int> AddTickets(List<ApplicationSql> sqlApplications, List<ApplicationClubSql> sqlApplicationClubs)
    {
        var sqlApplicationsCount = sqlApplications.Count;
        ConsoleEx.StartProgress("Adding tickets + requests: ");
        var added = 0;
        for (int index = 0; index < sqlApplicationsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var sqlApplication = sqlApplications[index];
            var sqlAppClubs = sqlApplicationClubs
                .Where(ac => ac.OldSystemApplicationId == sqlApplication.OldSystemApplicationId).ToList();
            var clubsRequested = String.Join(",", sqlAppClubs
                .Select(ac => $"S{(ac.Season ?? 0):0}:" +
                    ac.AgeLevel switch
                    {
                        AgeLevel.GradesK2 => "K2",
                        AgeLevel.Grades35 => "35",
                        null => "??",
                        _ => throw new ArgumentOutOfRangeException()
                    } + ":" +
                    ac.StartsOn?.ToString("yyyy-MM-dd")));

            var ticketStatus = sqlApplication.StatusString switch
            {
                "Received" => TicketStatus.Requested,
                "Pending" => TicketStatus.Pending,
                "Approved" => TicketStatus.Complete,
                "Denied" => TicketStatus.Denied,
                "Deleted" => TicketStatus.Archived,
                "Future" => TicketStatus.Potential,
                _ => throw new ArgumentOutOfRangeException()
            };

            var request = new RequestDb()
            {
                WorkshopCode = sqlApplication.WorkshopCode,
                ReferenceSource = null,
                ReferenceSourceOther = null,
                AppointmentId = sqlApplication.AppointmentId,
                ClubsRequested = clubsRequested,
                Comments = sqlApplication.Comments,
                SubmittedOn = sqlApplication.SubmittedOn ?? throw new Exception("Missing Submitted On"),
                CreatedOn = sqlApplication.CreatedOn
            };
            dbContext.Requests.Add(request);

            // now create the ticket with the person and place
            var ticket = new TicketDb()
            {
                Status = ticketStatus,
                Request = request,
                CreatedOn = sqlApplication.CreatedOn
            };
            if (!String.IsNullOrEmpty(sqlApplication.Notes))
                ticket.Notes = (List<TicketNoteDb>)
                [
                    new() {
                        Author = SoftCrowConstants.Display.System,
                        Content = sqlApplication.Notes }
                ];

            dbContext.Tickets.Add(ticket);
            await dbContext.SaveChangesAsync();
            added++;

            //OldSystemApplicationId = sqlApplication.OldSystemApplicationId,
            if (sqlApplication.OldSystemApplicationId != null)
            {
                var oldNewApplication = new OldNewDb()
                {
                    OldTableName = ApplicationTable,
                    OldId = sqlApplication.OldSystemApplicationId!.Value,
                    NewTableName = nameof(TicketDb),
                    NewId = ticket.TicketId
                };
                dbContext.OldNews.Add(oldNewApplication);
            }

            //OldSystemAddressId = sqlApplication.OldSystemAddressId,
            if (sqlApplication.OldSystemAddressId != null)
            {
                var oldNewAddress = new OldNewDb()
                {
                    OldTableName = PostalAddressTable,
                    OldId = sqlApplication.OldSystemAddressId!.Value,
                    NewTableName = nameof(TicketDb),
                    NewId = ticket.TicketId
                };
                dbContext.OldNews.Add(oldNewAddress);
            }

            foreach (var sqlAppClub in sqlAppClubs)
            {
                var oldNewAppClub = new OldNewDb()
                {
                    OldTableName = ApplicationClubTable,
                    OldId = sqlAppClub.OldSystemApplicationClubId!.Value,
                    NewTableName = nameof(TicketDb),
                    NewId = ticket.TicketId
                };
                dbContext.OldNews.Add(oldNewAppClub);
            }

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }

    private async Task<int> AddOrders(List<OrderSql> sqlOrders, List<OrderTrackerSql> sqlOrderTrackers, List<AddressSql> sqlAddresses)
    {
        var sqlOrdersCount = sqlOrders.Count;
        ConsoleEx.StartProgress("Adding orders & shipments: ");
        var added = 0;
        for (int index = 0; index < sqlOrdersCount; index++)
        {
            var sqlOrder = sqlOrders[index];
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            // start by finding the original address
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
            await dbContext.SaveChangesAsync();
            added++;

            //OldSystemOrderId = sqlOrder.OldSystemOrderId,
            if (sqlOrder.OldSystemOrderId != null)
            {
                var oldNewOrder = new OldNewDb()
                {
                    OldTableName = OrderTable,
                    OldId = sqlOrder.OldSystemOrderId!.Value,
                    NewTableName = nameof(OrderDb),
                    NewId = order.OrderId
                };
                dbContext.OldNews.Add(oldNewOrder);
            }

            //OldSystemShippingAddressId = sqlOrder.OldSystemShippingAddressId,
            if (sqlOrder.OldSystemShippingAddressId != null)
            {
                var oldNewAddress = new OldNewDb()
                {
                    OldTableName = PostalAddressTable,
                    OldId = sqlOrder.OldSystemShippingAddressId!.Value,
                    NewTableName = nameof(OrderDb),
                    NewId = order.OrderId
                };
                dbContext.OldNews.Add(oldNewAddress);
            }

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlOrdersCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }

    private async Task<(int, int)> AddClubs(List<ClubSql> sqlClubs,
        OrderLookup orderIdLookup, PlaceLookup placeIdLookup, KitLookup kitIdLookup)
    {
        var clubGroups = sqlClubs.GroupBy(c => c.OldSystemClubId).ToList();

        var sqlClubsCount = clubGroups.Count;
        ConsoleEx.StartProgress("Adding clubs with orders, kits & places: ");

        var added = 0;
        var mismatched = 0;
        for (var index = 0; index < sqlClubsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var clubGroup = clubGroups[index];
            var sqlClub = clubGroup.First(); // only difference will be the orders

            if (sqlClub.OldSystemOrganizationId == null)
                throw new UnreachableException("SqlClub missing OldSystemOrganizationId");

            // determine the season & age-level
            var season = sqlClub.Season ??
                         throw new UnreachableException("Club does not have a season.");
            var ageLevel = sqlClub.AgeLevel ??
                           throw new UnreachableException("Club does not have an age level");
            var mismatchNote = (String?)null;

            // find the matching kit
            KitDb? matchingKit;
            List<OrderDb> orders = [];
            if (sqlClub.OldSystemOrderId == null)
            {
                // find the closest kit using the lookup (start with getting lower years)
                int kitId = 0;
                for (var year = Int32.Parse(sqlClub.CreatedOn.ToString("yy")); year >= 12; year--)
                {
                    var key = $"S{season}-{((ageLevel == AgeLevel.GradesK2) ? "K2" : "35")}-{year}";
                    if (kitIdLookup.TryGetValue(key, out kitId)) break;
                }
                // next try the higher years instead
                if (kitId == 0)
                {
                    for (var year = Int32.Parse(sqlClub.CreatedOn.ToString("yy")) + 1; year <= 24; year++)
                    {
                        var key = $"S{season}-{((ageLevel == AgeLevel.GradesK2) ? "K2" : "35")}-{year}";
                        if (kitIdLookup.TryGetValue(key, out kitId)) break;
                    }
                }
                if (kitId == 0)
                    throw new UnreachableException(
                        $"'S{season}-{((ageLevel == AgeLevel.GradesK2) ? "K2" : "35")}-{sqlClub.CreatedOn:yy}' not found");

                matchingKit = await dbContext.Kits.FirstOrDefaultAsync(k => k.KitId == kitId) ??
                                  throw new UnreachableException($"Could not find kit Id#:{kitId}");
                mismatchNote = "No matching order, so Kit year is assumed.";
            }
            else
            {
                foreach (var oldSystemOrderId in clubGroup.Select(cg => cg.OldSystemOrderId))
                {
                    var orderId = orderIdLookup.GetValueOrDefault(oldSystemOrderId!.Value);
                    var order = await dbContext.Orders
                                    .Include(o => o.OrderOffers)
                                    .ThenInclude(oo => oo.Offer)
                                    .ThenInclude(oo => oo.Kits)
                                    .AsSingleQuery()
                                    .FirstOrDefaultAsync(o => o.OrderId == orderId) ??
                                throw new UnreachableException($"Could not find order #:{orderId}");
                    orders.Add(order);
                }

                var kits = orders
                    .SelectMany(o => o.OrderOffers)
                    .SelectMany(oo => oo.Offer.Kits)
                    .ToList();
                if (!kits.Any())
                    throw new UnreachableException("Order did not have any kits.");

                matchingKit = kits.FirstOrDefault(k => k.AgeLevel == ageLevel && k.Season == season);
                if (matchingKit == null)
                {
                    mismatched++;
                    if (kits.Any(k => k.Season == season))
                    {
                        var distinct = String.Join(", ", kits.Select(k => k.AgeLevel.GetLabel()).Distinct());
                        mismatchNote = $"Order age level doesn't match club age level: {distinct} != {ageLevel.GetLabel()}";
                        matchingKit = kits.First(k => k.Season == season);
                    }
                    else if (kits.Any(k => k.AgeLevel == ageLevel))
                    {
                        var distinct = String.Join(", ", kits.Select(k => k.Season).Distinct());
                        mismatchNote = $"Order season doesn't match club season: {distinct} != {season}";
                        matchingKit = kits.First(k => k.AgeLevel == ageLevel);
                    }
                    else
                    {
                        var distinct = String.Join(", ", kits.Select(k => $"S{k.Season} - {k.AgeLevel.GetLabel()}").Distinct());
                        mismatchNote = $"Order season doesn't match club season: {distinct} != S{season} - {ageLevel.GetLabel()}";
                        matchingKit = kits.First();
                    }
                }
            }

            // find the right place
            var placeId = placeIdLookup.GetValueOrDefault(sqlClub.OldSystemOrganizationId!.Value);
            var place = dbContext.Places.FirstOrDefault(p => p.PlaceId == placeId) ??
                        throw new UnreachableException($"Could not find place #{placeId}"); ;

            // now create the club itself
            var clubStatus = sqlClub.StartsOnDateTime == null ? ClubStatus.Archived :
                sqlClub.StartsOnDateTime < DateTime.Today ? ClubStatus.Complete : ClubStatus.Active;
            var club = new ClubDb()
            {
                Status = clubStatus,
                Kit = matchingKit,
                Place = place,
                StartsOn = sqlClub.StartsOn,
                CreatedOn = sqlClub.CreatedOn,
                Orders = orders
            };
            club.Notes ??= new List<ClubNoteDb>();
            if (!String.IsNullOrEmpty(sqlClub.Notes))
                club.Notes.Add(new()
                {
                    Author = SoftCrowConstants.Display.System,
                    Content = sqlClub.Notes
                });
            if (!String.IsNullOrEmpty(mismatchNote))
                club.Notes.Add(new()
                {
                    Author = SoftCrowConstants.Display.System,
                    Content = mismatchNote
                });

            dbContext.Clubs.Add(club);

            await dbContext.SaveChangesAsync();
            added++;

            //OldSystemClubId = sqlClub.OldSystemClubId,
            if (sqlClub.OldSystemClubId != null)
            {
                var oldNewClub = new OldNewDb()
                {
                    OldTableName = ClubTable,
                    OldId = sqlClub.OldSystemClubId!.Value,
                    NewTableName = nameof(ClubDb),
                    NewId = club.ClubId
                };
                dbContext.OldNews.Add(oldNewClub);
            }

            //OldSystemOrganizationId = sqlClub.OldSystemOrganizationId,
            //OldSystemCoachId = sqlClub.OldSystemCoachId,
            //OldSystemMeetingAddressId = sqlClub.OldSystemMeetingAddressId,

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlClubsCount);
        }
        ConsoleEx.EndProgress();

        return (added, mismatched);
    }

    private async Task<(int,int)> AddKitPages()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // determine the kit pages to add, based on the kits
        var kits = await dbContext.Kits.Where(k => k.Year == "F23" || k.Year == "F24").ToListAsync();

        var kitsCount = kits.Count;
        ConsoleEx.StartProgress("Adding kit pages: ");
        var added = 0;
        var skipped = 0;
        for (int index = 0; index < kitsCount; index++)
        {
            var kit = kits[index];

            var kitPageUrl = $"/kit-page/{kit.Key.ToSlug()}";
            var existing = await dbContext.KitPages
                .FirstOrDefaultAsync(kp => kp.Url == kitPageUrl);
            if (existing != null) { skipped++; continue; }

            var kitPage = new KitPageDb()
            {
                Kits = [kit],
                Status = KitPageStatus.Active,
                Url = kitPageUrl,
                Title = GetTitleFromKit(kit),
                CreatedOn = DateTime.Now
            };
            dbContext.KitPages.Add(kitPage);
            added++;
            await dbContext.SaveChangesAsync();

            ConsoleEx.ShowProgress((float)index / (float)kitsCount);
        }
        ConsoleEx.EndProgress();
        return (added, skipped);
    }
    
    private async Task<(int, int)> AddPermissions(List<int> personIds)
    {
        var personsCount = personIds.Count;

        ConsoleEx.StartProgress("Adding permissions: ");
        var added = 0;
        var skipped = 0;
        for (int index = 0; index < personsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var personId = personIds[index];
            var person = await dbContext.Persons
                .Include(p => p.ClubPersons)
                .ThenInclude(cp => cp.Club)
                .ThenInclude(c => c.Kit)
                .Include(p => p.Permissions)
                .ThenInclude(p => p.KitPage)
                .AsSingleQuery()
                .FirstOrDefaultAsync(p => p.PersonId == personId) ??
                     throw new UnreachableException($"Could not find person: ID#{personId}");

            var shouldHave = person.ClubPersons
                .Select(cp => cp.Club.Kit.KitPageId)
                .Where(kpi => kpi != null)
                .Cast<int>() ?? throw new UnreachableException("Could not get kit ids for person");
            var doesHave = person.Permissions
                .Select(p => p.KitPageId) ?? throw new UnreachableException("Could not get permission ids for person");

            var toAdd = shouldHave.Except(doesHave).ToList();
            if (!toAdd.Any()) { skipped++;  }

            else
            {
                var permissions = toAdd
                    .Select(a => new PermissionDb() { Person = person, KitPageId = a });
                dbContext.Permissions.AddRange(permissions);

                added++;
                await dbContext.SaveChangesAsync();
            }

            ConsoleEx.ShowProgress((float)index / (float)personsCount);
        } 
        ConsoleEx.EndProgress();
        return (added, skipped);
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

    private async Task<(int, int, int)> JoinPersonsToTickets(List<ApplicationSql> sqlApplications,
        TicketLookup ticketIdLookup, PersonLookup personIdLookup)
    {
        var sqlApplicationsCount = sqlApplications.Count;
        ConsoleEx.StartProgress("Adding persons to tickets: ");
        var added = 0;
        var existing = 0;
        var matched = 0;
        for (int index = 0; index < sqlApplicationsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var sqlApplication = sqlApplications[index];

            // find the right ticket
            if (!ticketIdLookup.TryGetValue(sqlApplication.OldSystemApplicationId!.Value, out var ticketId))
                throw new Exception($"Could not find ticket for application: {sqlApplication.OldSystemApplicationId}");
            var ticket = dbContext.Tickets.FirstOrDefault(t => t.TicketId == ticketId) ??
                         throw new Exception($"Could not find ticket for id: {ticketId}");

            // find the right person
            PersonDb? person = null;
            if (sqlApplication.OldSystemLinkedCoachId != null)
            {
                var personId = personIdLookup.GetValueOrDefault(sqlApplication.OldSystemLinkedCoachId!.Value);
                person = dbContext.Persons.FirstOrDefault(p => p.PersonId == personId);
                existing++;
            }

            if (person == null)
            {
                person = dbContext.Persons.FirstOrDefault(p => p.Email == sqlApplication.ApplicantEmail);
                matched++;
            }

            if (person == null)
            {
                person = new PersonDb()
                {
                    FirstName = sqlApplication.ApplicantFirstName,
                    LastName = sqlApplication.ApplicantLastName,
                    Email = sqlApplication.ApplicantEmail,
                    TimeZone = sqlApplication.ApplicantTimeZone,
                    Phone = sqlApplication.ApplicantPhone +
                            (String.IsNullOrEmpty(sqlApplication.ApplicantPhoneExt)
                                ? null
                                : $" x{sqlApplication.ApplicantPhoneExt}"),
                    JobTitle = JobTitleFromRole(sqlApplication.ApplicantRole),
                    JobTitleOther = JobTitleOtherFromRole(sqlApplication.ApplicantRole),
                    CreatedOn = sqlApplication.CreatedOn
                };
                dbContext.Persons.Add(person);
                added++;
            }

            ticket.TicketPersons ??= new List<TicketPersonDb>();
            ticket.TicketPersons.Add(new() { Person = person, Ticket = ticket, Ordinal = 0 });

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
        }
        ConsoleEx.EndProgress();

        return (added, existing, matched);
    }

    private async Task<(int, int, int)> JoinPlacesToTickets(List<ApplicationSql> sqlApplications, List<AddressSql> sqlAddresses,
        TicketLookup ticketIdLookup, PlaceLookup placeIdLookup)
    {
        var sqlApplicationsCount = sqlApplications.Count;
        ConsoleEx.StartProgress("Adding places to tickets: ");
        var added = 0;
        var existing = 0;
        var skipped = 0;
        for (int index = 0; index < sqlApplicationsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var sqlApplication = sqlApplications[index];

            // find the right ticket
            if (!ticketIdLookup.TryGetValue(sqlApplication.OldSystemApplicationId!.Value, out var ticketId))
                throw new Exception($"Could not find ticket for application: {sqlApplication.OldSystemApplicationId}");
            var ticket = dbContext.Tickets.FirstOrDefault(t => t.TicketId == ticketId) ??
                         throw new Exception($"Could not find ticket for id: {ticketId}");

            // find the right place
            PlaceDb? place = null;
            if (sqlApplication.OldSystemLinkedCoachId != null)
            {
                var placeId = placeIdLookup.GetValueOrDefault(sqlApplication.OldSystemLinkedCoachId!.Value);
                place = dbContext.Places.FirstOrDefault(p => p.PlaceId == placeId);
                existing++;
            }

            if (place == null
                && sqlApplication.OldSystemAddressId != null
                && !String.IsNullOrEmpty(sqlApplication.OrganizationName)
                && sqlApplication.OrganizationType != null)
            {
                var sqlAddress = sqlAddresses.FirstOrDefault(a => a.OldSystemUsaPostalId == sqlApplication.OldSystemAddressId) ??
                                 throw new Exception("Missing address on application.");
                place = new PlaceDb()
                {
                    Name = sqlApplication.OrganizationName,
                    Line1 = sqlAddress.StreetAddress ?? SoftCrowConstants.Display.NotSet,
                    Line2 = null,
                    City = sqlAddress.City ?? SoftCrowConstants.Display.NotSet,
                    State = sqlAddress.State ?? SoftCrowConstants.Display.NotSet,
                    ZIPCode = sqlAddress.PostalCode ?? SoftCrowConstants.Display.NotSet,
                    IsMilitary = sqlAddress.IsMilitary,
                    Type = sqlApplication.OrganizationType.Value,
                    TypeOther = sqlApplication.OrganizationTypeOther,
                    TaxIdentifier = sqlApplication.OrganizationTaxIdentifier,
                    CreatedOn = sqlApplication.CreatedOn
                };
                dbContext.Places.Add(place);
            }

            if (place == null)
            {
                skipped++;
                continue;
            }

            ticket.Place = place;

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
        }
        ConsoleEx.EndProgress();

        return (added, existing, skipped);
    }

    private async Task<int> JoinOffersToOrders(List<OrderSkuSql> sqlOrderSkus,
        OrderLookup orderIdLookup, OfferLookup offerIdLookup)
    {
        var sqlOrderSkusCount = sqlOrderSkus.Count;
        ConsoleEx.StartProgress("Joining offers to orders: ");

        var joined = 0;
        for (int index = 0; index < sqlOrderSkusCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();
            var sqlOrderSku = sqlOrderSkus[index];

            if (sqlOrderSku.OldSystemOrderId == null)
                throw new UnreachableException("SqlOrderSku has no OldSystemOrderId");
            if (sqlOrderSku.OldSystemSkuId == null)
                throw new UnreachableException("SqlOrderSku has no OldSystemSkuId");

            // find the right order
            var orderId = orderIdLookup.GetValueOrDefault(sqlOrderSku.OldSystemOrderId!.Value);
            var order = dbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new UnreachableException($"Could not find order: {orderId}");

            // find the right offer
            var offerId = offerIdLookup.GetValueOrDefault(sqlOrderSku.OldSystemSkuId!.Value);
            var offer = dbContext.Offers.FirstOrDefault(o => o.OfferId == offerId);
            if (offer == null)
                throw new UnreachableException($"Could not find offer: {offerId}");

            // join them together
            var orderOffer = new OrderOfferDb()
            {
                Order = order,
                Offer = offer,
                Ordinal = sqlOrderSku.Ordinal,
                Quantity = sqlOrderSku.Quantity
            };
            dbContext.OrderOffers.Add(orderOffer);
            await dbContext.SaveChangesAsync();
            joined++;

            //public Guid? OldSystemOrderSkuId { get; set; } = null;
            if (sqlOrderSku.OldSystemOrderSkuId != null)
            {
                var oldNewAddress = new OldNewDb()
                {
                    OldTableName = OrderSkuTable,
                    OldId = sqlOrderSku.OldSystemOrderSkuId!.Value,
                    NewTableName = nameof(OrderOfferDb),
                    NewId = orderOffer.OrderOfferId
                };
                dbContext.OldNews.Add(oldNewAddress);
            }

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlOrderSkusCount);
        }
        ConsoleEx.EndProgress();

        return joined;
    }

    private async Task<(int, int)> JoinPersonsToClubs(List<ClubSql> sqlClubs, ClubLookup clubIdLookup, PersonLookup personIdLookup)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var sqlClubsCount = sqlClubs.Count;
        ConsoleEx.StartProgress("Joining persons to clubs: ");

        var joined = 0;
        var skipped = 0;
        for (int index = 0; index < sqlClubsCount; index++)
        {
            var sqlClub = sqlClubs[index];

            // find the right club
            var clubId = clubIdLookup.GetValueOrDefault(sqlClub.OldSystemClubId!.Value);
            var club = dbContext.Clubs
                .Include(c => c.ClubPersons)
                .ThenInclude(cp => cp.Person)
                .FirstOrDefault(o => o.ClubId == clubId);
            if (club == null)
                throw new UnreachableException($"Could not find club: {clubId}");

            // find the right person
            var personId = personIdLookup.GetValueOrDefault(sqlClub.OldSystemCoachId!.Value);
            if (personId == 0)
            {
                var deletedCoach = await oldSystemService.GetDeletedCoach(sqlClub.OldSystemCoachId!.Value) ??
                                   throw new UnreachableException($"Could not find deleted coach: {sqlClub.OldSystemCoachId}");

                personId =
                    (await dbContext.Persons.FirstOrDefaultAsync(p => p.Email == deletedCoach.Email))?.PersonId ??
                    throw new UnreachableException("Deleted coach had no matching coach with same email.");
            }

            var person = dbContext.Persons.FirstOrDefault(o => o.PersonId == personId) ??
                         throw new UnreachableException($"Could not find person: {personId}");

            // if we've already got people, skip
            var ordinal = 0;
            if (club.ClubPersons.Any())
            {
                var newPersons = club.ClubPersons.Select(cp => cp.Person).Where(p => p.PersonId != personId).ToList();
                if (!newPersons.Any())
                {
                    skipped++; 
                    ConsoleEx.ShowProgress((float)index / (float)sqlClubsCount);
                    continue;
                }
                ordinal = club.ClubPersons.Count + 1;
            }

            // join them together
            var clubPerson = new ClubPersonDb()
            {
                Club = club,
                Person = person,
                Ordinal = ordinal
            };
            dbContext.ClubPersons.Add(clubPerson);
            joined++;

            if (index % SaveBlock == 0)
                await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlClubsCount);
        }
        await dbContext.SaveChangesAsync();
        ConsoleEx.EndProgress();

        return (joined, skipped);
    }
    #endregion

    #region Remove Dupes
    private async Task RemoveExistingOrganizations(List<OrganizationSql> sqlOrganizations)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(PlaceDb) && o.OldTableName == OrganizationTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlOrganizations.RemoveAll(m => existingGuids.Contains(m.OldSystemOrganizationId!.Value));
    }

    private async Task RemoveExistingCoaches(List<CoachSql> sqlCoaches)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(PersonDb) && o.OldTableName == CoachTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlCoaches.RemoveAll(m => existingGuids.Contains(m.OldSystemCoachId!.Value));
    }

    private async Task RemoveExistingTickets(List<ApplicationSql> sqlApplications)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(TicketDb) && o.OldTableName == ApplicationTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlApplications.RemoveAll(m => existingGuids.Contains(m.OldSystemApplicationId!.Value));
    }

    private async Task RemoveExistingSkus(List<SkuSql> sqlSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(OfferDb) && o.OldTableName == SkuTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlSkus.RemoveAll(m => existingGuids.Contains(m.OldSystemSkuId!.Value));
    }

    private async Task RemoveExistingOrders(List<OrderSql> sqlOrders)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(OrderDb) && o.OldTableName == OrderTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlOrders.RemoveAll(m => existingGuids.Contains(m.OldSystemOrderId!.Value));
    }

    private async Task RemoveExistingOrderSkus(List<OrderSkuSql> sqlOrderSkus)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(OrderOfferDb) && o.OldTableName == OrderSkuTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlOrderSkus.RemoveAll(m => existingGuids.Contains(m.OldSystemOrderSkuId!.Value));
    }

    private async Task RemoveExistingClubs(List<ClubSql> sqlClubs)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(ClubDb) && o.OldTableName == ClubTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlClubs.RemoveAll(m => existingGuids.Contains(m.OldSystemClubId!.Value));
    }
    #endregion

    #region Lookups
    private async Task<ClubLookup> CreateClubsLookup()
    {
        var clubIdLookup = new ClubLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for clubs given the old system id
        var allClubs = await dbContext.Clubs.AsNoTracking().ToListAsync();
        var oldNewClubs = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(ClubDb) && o.OldTableName == ClubTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating clubs lookup: ");
        var index = 0;
        foreach (var oldNewClub in oldNewClubs)
        {
            var club = allClubs.FirstOrDefault(p => p.ClubId == oldNewClub.NewId);
            if (club != null) clubIdLookup.Add(oldNewClub.OldId, club.ClubId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewClubs.Count);
        }
        ConsoleEx.EndProgress();

        return clubIdLookup;
    }

    private async Task<PersonLookup> CreatePersonsLookup()
    {
        var personIdLookup = new PersonLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for persons given the old system id
        var allPersons = await dbContext.Persons.AsNoTracking().ToListAsync();
        var oldNewPersons = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(PersonDb) && o.OldTableName == CoachTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating persons lookup: ");
        var index = 0;
        foreach (var oldNewPerson in oldNewPersons)
        {
            var person = allPersons.FirstOrDefault(p => p.PersonId == oldNewPerson.NewId);
            if (person != null) personIdLookup.Add(oldNewPerson.OldId, person.PersonId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewPersons.Count);
        }
        ConsoleEx.EndProgress();

        return personIdLookup;
    }

    private async Task<PlaceLookup> CreatePlacesLookup()
    {
        var placeIdLookup = new PlaceLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for places given the old system id
        var allPlaces = await dbContext.Places.AsNoTracking().ToListAsync();
        var oldNewPlaces = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(PlaceDb) && o.OldTableName == OrganizationTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating places lookup: ");
        var index = 0;
        foreach (var oldNewPlace in oldNewPlaces)
        {
            var place = allPlaces.FirstOrDefault(p => p.PlaceId == oldNewPlace.NewId);
            if (place != null) placeIdLookup.Add(oldNewPlace.OldId, place.PlaceId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewPlaces.Count);
        }
        ConsoleEx.EndProgress();

        return placeIdLookup;
    }

    private async Task<TicketLookup> CreateTicketsLookup()
    {
        var ticketIdLookup = new TicketLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for tickets given the old system application id
        var allTickets = await dbContext.Tickets.AsNoTracking().ToListAsync();
        var oldNewTickets = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(TicketDb) && o.OldTableName == ApplicationTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating tickets lookup: ");
        var index = 0;
        foreach (var oldNewTicket in oldNewTickets)
        {
            var ticket = allTickets.FirstOrDefault(t => t.TicketId == oldNewTicket.NewId);
            if (ticket != null) ticketIdLookup.Add(oldNewTicket.OldId, ticket.TicketId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewTickets.Count);
        }
        ConsoleEx.EndProgress();

        return ticketIdLookup;
    }

    private async Task<OfferLookup> CreateOffersLookup()
    {
        var offerIdLookup = new OfferLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for offers given the old system id
        var allOffers = await dbContext.Offers.AsNoTracking().ToListAsync();
        var oldNewOffers = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(OfferDb) && o.OldTableName == SkuTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating offers lookup: ");
        var index = 0;
        foreach (var oldNewOffer in oldNewOffers)
        {
            var offer = allOffers.FirstOrDefault(p => p.OfferId == oldNewOffer.NewId);
            if (offer != null) offerIdLookup.Add(oldNewOffer.OldId, offer.OfferId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewOffers.Count);
        }
        ConsoleEx.EndProgress();

        return offerIdLookup;
    }

    private async Task<OrderLookup> CreateOrdersLookup()
    {
        var orderIdLookup = new OrderLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for orders given the old system coach id
        var allOrders = await dbContext.Orders.AsNoTracking().ToListAsync();
        var oldNewOrders = await dbContext.OldNews.AsNoTracking()
            .Where(o => o.NewTableName == nameof(OrderDb) && o.OldTableName == OrderTable)
            .ToListAsync();

        ConsoleEx.StartProgress("Creating orders lookup: ");
        var index = 0;
        foreach (var oldNewOrder in oldNewOrders)
        {
            var order = allOrders.FirstOrDefault(p => p.OrderId == oldNewOrder.NewId);
            if (order != null) orderIdLookup.Add(oldNewOrder.OldId, order.OrderId);
            ConsoleEx.ShowProgress((float)index++ / (float)oldNewOrders.Count);
        }
        ConsoleEx.EndProgress();

        return orderIdLookup;
    }

    private async Task<KitLookup> CreateKitsLookup()
    {
        var kitIdLookup = new KitLookup();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for kits given the old system coach id
        var allKits = await dbContext.Kits.AsNoTracking().ToListAsync();

        ConsoleEx.StartProgress("Creating kits lookup: ");
        var index = 0;
        foreach (var kit in allKits.OrderByDescending(k => k.CreatedOn))
        {
            var parts = kit.Key.Split('.');
            var season = parts[1];
            var year = Int32.Parse(parts[2].Substring(1));
            var ageLevel = parts[^1];
            var newKey = $"{season}-{ageLevel}-{year}";
            kitIdLookup.TryAdd(newKey, kit.KitId);

            ConsoleEx.ShowProgress((float)index++ / (float)allKits.Count);
        }
        ConsoleEx.EndProgress();

        return kitIdLookup;
    }
    #endregion

    #region Private Methods
    private JobTitle? JobTitleFromRole(string? role)
    {
        if (String.IsNullOrWhiteSpace(role)) return null;
        if (role.ToLower().Contains("teacher")) return JobTitle.Teacher;
        if (role.ToLower().Contains("supervisor")) return JobTitle.Supervisor;
        if (role.ToLower().Contains("librarian")) return JobTitle.Librarian;
        if (role.ToLower().Contains("principal")) return JobTitle.Principal;
        if (role.ToLower().Contains("superintendent")) return JobTitle.Superintendent;
        return JobTitle.Other;
    }
    private string? JobTitleOtherFromRole(string? role)
    {
        if (String.IsNullOrWhiteSpace(role)) return null;
        if (role.Trim().ToLower() == "teacher") return null;
        if (role.Trim().ToLower() == "supervisor") return null;
        if (role.Trim().ToLower() == "librarian") return null;
        if (role.Trim().ToLower() == "principal") return null;
        if (role.Trim().ToLower() == "superintendent") return null;
        return role;
    }

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

    private string GetFulcoIdFromSqlSky(SkuSql sqlSku)
    {
        var fulcoId = (!sqlSku.Key.Contains('-')) ? sqlSku.Key : sqlSku.Key.Substring(0, sqlSku.Key.IndexOf('-'));
        var version = GetVersionFromSqlSku(sqlSku);
        if (!String.IsNullOrWhiteSpace(version) && !fulcoId.EndsWith(version))
            fulcoId += $"{version}";
        return fulcoId;
    }

    private static readonly List<string> GradeStrings = [
        @",\s*K\s*-\s*2nd\s*Grade",
        @",\s*3rd\s*-\s*5th\s*Grade",
        @"GRADES\s*K-2",
        @"GRADES\s*3-5",
        @"\-\s*Grades\s*K-2",
        @"\-\s*Grades\s*3-5"
    ];
    private static string DropGradesFromTitle(string title)
    {
        foreach (var gradeString in GradeStrings)
        {
            title = Regex
                .Replace(title, gradeString, "", RegexOptions.IgnoreCase)
                .Replace("  ", " ")
                .Trim();
        }
        return title;
    }

    private static string GetKeyFromData(string year, int season, AgeLevel ageLevel, string? version) =>
        String.Join(".", (new List<string?>() {
                "C8",
                $"S{season}",
                year,
                version,
                ageLevel switch
                {
                    AgeLevel.GradesK2 => "K2",
                    AgeLevel.Grades35 => "35",
                    _ => throw new ArgumentOutOfRangeException(nameof(ageLevel), ageLevel, null)
                }
            }).
            Where(s => !String.IsNullOrEmpty(s)));

    private static string GetTitleFromKit(KitDb kit)
    {
        var seasonString = $"Season {kit.Season}{kit.Version}";
        var ageLevelString = kit.AgeLevel switch {
            AgeLevel.GradesK2 => "K-2nd Grade",
            AgeLevel.Grades35 => "3rd-5th Grade",
            _ => throw new ArgumentOutOfRangeException()
        };
        var year = Int32.Parse(kit.Year.Substring(1));
        var yearString = $"(20{year}-{year+1})";

        return $"Crazy 8s {seasonString}, {ageLevelString} {yearString}";
    }
    #endregion

    #region Private Classes
    public class ClubLookup : Dictionary<Guid, int> { }
    public class PersonLookup : Dictionary<Guid, int> { }
    public class PlaceLookup : Dictionary<Guid, int> { }
    public class TicketLookup : Dictionary<Guid, int> { }
    public class OfferLookup : Dictionary<Guid, int> { }
    public class OrderLookup : Dictionary<Guid, int> { }
    public class KitLookup : Dictionary<string, int> { }
    #endregion
}