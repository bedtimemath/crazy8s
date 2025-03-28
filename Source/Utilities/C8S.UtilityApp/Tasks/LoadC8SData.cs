using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using C8S.Domain.EFCore.Contexts;
using C8S.Domain.EFCore.Models;
using C8S.Domain.Enums;
using C8S.Domain.Features.Requests.Enums;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Models;
using C8S.UtilityApp.Services;
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

        /*** SET UP OFFERS ***/
        var sqlSkus = (await oldSystemService.GetSkus())
            .Where(s => s.Status != OfferStatus.Draft)
            .ToList();
        logger.LogInformation("Found {Count:#,##0} skus", sqlSkus.Count);

        await RemoveExistingSkus(sqlSkus);
        logger.LogInformation("Removed existing, now {Count:#,##0} skus", sqlSkus.Count);

        if (sqlSkus.Count > 0)
        {
            var offersAdded = await CreateOffersFromSkus(sqlSkus);
            logger.LogInformation("Added {Count:#,##0} offers.", offersAdded);
        }
        
        /*** SET UP KITS ***/


        return 0;

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
            var sqlApplicationsCount = await AddTickets(sqlApplications);
            logger.LogInformation("Added {Count:#,##0} tickets + requests", sqlApplicationsCount);

            var personIdLookup = await CreatePersonsLookup();
            var placeIdLookup = await CreatePlacesLookup();
            var ticketIdLookup = await CreateTicketsLookup();

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

        sqlApplications = (await oldSystemService.GetApplications()).ToList(); // todo: remove this line

        /*** APPLICATION CLUBS ***/
        var sqlApplicationClubs = (await oldSystemService.GetApplicationClubs()).ToList();
        logger.LogInformation("Found {Count:#,##0} application clubs", sqlApplicationClubs.Count);

        await RemoveExistingTicketClubs(sqlApplicationClubs);
        logger.LogInformation("Removed existing, now {Count:#,##0} application clubs", sqlApplicationClubs.Count);

#if false
        /*** ADDING REQUESTED CLUBS ***/
        if (sqlApplicationClubs.Count > 0)
        {
            var requestedClubsAdded = await AddTicketClubs(sqlApplicationClubs);
            logger.LogInformation("Added {Count:#,##0} request clubs", requestedClubsAdded);
        } 
#endif


#if false
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
            
#if false
            var kit = new KitDb()
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
                Year = GetYearFromSqlSku(sqlSku),
                Season = sqlSku.Season!.Value,
                AgeLevel = sqlSku.AgeLevel!.Value,
                Version = GetVersionFromSqlSku(sqlSku),
                Comments = sqlSku.Notes,
                CreatedOn = sqlSku.CreatedOn
            };
            dbContext.Kits.Add(kit); 
#endif

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
                person = new PersonDb()
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

    private async Task<int> AddTickets(List<ApplicationSql> sqlApplications)
    {
        var sqlApplicationsCount = sqlApplications.Count;
        ConsoleEx.StartProgress("Adding tickets / requests: ");
        var added = 0;
        for (int index = 0; index < sqlApplicationsCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var sqlApplication = sqlApplications[index];

            var ticketStatus = sqlApplication.Status switch
            {
                RequestStatus.Received => TicketStatus.Requested,
                RequestStatus.Pending => TicketStatus.Pending,
                RequestStatus.Approved => TicketStatus.Complete,
                RequestStatus.Denied => TicketStatus.Denied,
                RequestStatus.Deleted => TicketStatus.Archived,
                RequestStatus.Future => TicketStatus.Potential,
                _ => throw new ArgumentOutOfRangeException()
            };

            var request = new RequestDb()
            {
                WorkshopCode = sqlApplication.WorkshopCode,
                ReferenceSource = null,
                ReferenceSourceOther = null,
                AppointmentId = sqlApplication.AppointmentId,
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

            await dbContext.SaveChangesAsync();
            ConsoleEx.ShowProgress((float)index / (float)sqlApplicationsCount);
        }
        ConsoleEx.EndProgress();

        return added;
    }

    private async Task<int> AddTicketClubs(List<ApplicationClubSql> sqlApplicationClubs,
        Dictionary<Guid, int> ticketLookup)
    {
        throw new NotImplementedException();

#if false
        var sqlCount = sqlApplicationClubs.Count;
        ConsoleEx.StartProgress("Adding clubs from applications: ");
        var added = 0;
        for (int index = 0; index < sqlCount; index++)
        {
            await using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var appClub = sqlApplicationClubs[index];
            if (!ticketLookup.TryGetValue(appClub.OldSystemApplicationId!.Value, out var ticketId))
                throw new Exception($"Could not find ticket for application: {appClub.OldSystemApplicationId}");
            var ticket = dbContext.Tickets.FirstOrDefault(t => t.TicketId == ticketId) ??
                         throw new Exception($"Could not find ticket for id: {ticketId}");

            var requestedClub = new ClubDb()
            {
                RequestId = request.RequestId,
                OldSystemApplicationClubId = appClub.OldSystemApplicationClubId,
                OldSystemApplicationId = appClub.OldSystemApplicationId,
                OldSystemLinkedClubId = appClub.OldLinkedClubId,
                AgeLevel = appClub.AgeLevel ?? throw new Exception("Missing Age Level"),
                Season = appClub.Season ?? throw new Exception("Missing Season"),
                StartsOn = appClub.StartsOn ?? throw new Exception("Missing Starts On")
            };
            dbContext.Clubs.Add(requestedClub);
            added++;

            ConsoleEx.ShowProgress((float)index / (float)sqlCount);
        }
        ConsoleEx.EndProgress();

        return added; 
#endif
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
        Dictionary<Guid, int> ticketIdLookup, Dictionary<Guid, int> personIdLookup)
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
        Dictionary<Guid, int> ticketIdLookup, Dictionary<Guid, int> placeIdLookup)
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

    private async Task RemoveExistingTicketClubs(List<ApplicationClubSql> sqlApplicationClubs)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var existingGuids = await dbContext.OldNews
            .Where(o => o.NewTableName == nameof(ClubDb) && o.OldTableName == ApplicationClubTable)
            .Select(o => o.OldId)
            .ToListAsync();
        sqlApplicationClubs.RemoveAll(m => existingGuids.Contains(m.OldSystemApplicationId!.Value));
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
    #endregion

    #region Lookups
    private async Task<Dictionary<Guid, int>> CreatePersonsLookup()
    {
        var personIdLookup = new Dictionary<Guid, int>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for persons given the old system coach id
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

    private async Task<Dictionary<Guid, int>> CreatePlacesLookup()
    {
        var placeIdLookup = new Dictionary<Guid, int>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // create a lookup for places given the old system coach id
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

    private async Task<Dictionary<Guid, int>> CreateTicketsLookup()
    {
        var ticketIdLookup = new Dictionary<Guid, int>();
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

    private async Task RemoveExistingApplicationClubs(List<ApplicationClubSql> sqlApplicationClubs)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var existingApplicationClubIds = await dbContext.RequestedClubs
            .Select(o => o.OldSystemApplicationClubId)
            .ToListAsync();
        sqlApplicationClubs.RemoveAll(m => existingApplicationClubIds.Contains(m.OldSystemApplicationClubId));
    }
#endif

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

}