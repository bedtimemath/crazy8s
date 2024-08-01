using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Filters;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Addresses
    public async Task<IList<AddressDTO>> GetAddresses(
        AddressFilter? filter = null,
        int? startIndex = null, int? takeCount = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Addresses // clubs included automatically
                .OrderBy(a => a.StreetAddress)
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.StreetAddress.Contains(filter.Query)) ||
                                (a.City.Contains(filter.Query)) ||
                                (a.State.Contains(filter.Query)) );
            }
        }

        /* START & SKIP */
        if (startIndex != null)
            queryable = queryable.Skip(startIndex.Value);

        if (takeCount != null)
            queryable = queryable.Take(takeCount.Value);

        return (await queryable.ToListAsync())
            .Select(mapper.Map<AddressDTO>).ToList();
    }
    
    public async Task<int> GetAddressesCount(
        AddressFilter? filter = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Addresses // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.StreetAddress.Contains(filter.Query)) ||
                                (a.City.Contains(filter.Query)) ||
                                (a.State.Contains(filter.Query)) );
            }
        }

        return await queryable.CountAsync();
    }
    #endregion
    
    #region Applications
    public async Task<IList<ApplicationDTO>> GetApplications(
        ApplicationFilter? filter = null,
        int? startIndex = null, int? takeCount = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Applications // clubs included automatically
                .OrderByDescending(a => a.SubmittedOn)
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.ApplicantFirstName != null && a.ApplicantFirstName.Contains(filter.Query)) ||
                                (a.ApplicantLastName.Contains(filter.Query)) ||
                                (a.ApplicantEmail.Contains(filter.Query)) );
            }

            if (filter.Status != null)
            {
                queryable = queryable
                    .Where(a => a.Status == filter.Status);
            }
        }

        /* START & SKIP */
        if (startIndex != null)
            queryable = queryable.Skip(startIndex.Value);

        if (takeCount != null)
            queryable = queryable.Take(takeCount.Value);

        return (await queryable.ToListAsync())
            .Select(mapper.Map<ApplicationDTO>).ToList();
    }
    
    public async Task<int> GetApplicationsCount(
        ApplicationFilter? filter = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Applications // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.ApplicantFirstName != null && a.ApplicantFirstName.Contains(filter.Query)) ||
                                (a.ApplicantLastName.Contains(filter.Query)) ||
                                (a.ApplicantEmail.Contains(filter.Query)) );
            }

            if (filter.Status != null)
            {
                queryable = queryable
                    .Where(a => a.Status == filter.Status);
            }
        }

        return await queryable.CountAsync();
    }
    #endregion

    #region Application Clubs
    public async Task<IList<ApplicationClubDTO>> GetApplicationClubs()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.ApplicationClubs
                .AsNoTracking()
                .AsQueryable();

        return (await queryable.ToListAsync())
            .Select(mapper.Map<ApplicationClubDTO>).ToList();
    }
    #endregion
    
    #region Clubs
    public async Task<IList<ClubDTO>> GetClubs(
        ClubFilter? filter = null,
        int? startIndex = null, int? takeCount = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Clubs // clubs included automatically
                .OrderBy(a => a.StartsOn)
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                //queryable = queryable
                //    .Where(a => (a.FirstName.Contains(filter.Query)) ||
                //                (a.LastName.Contains(filter.Query)) ||
                //                (a.Email.Contains(filter.Query)) );
            }
        }

        /* START & SKIP */
        if (startIndex != null)
            queryable = queryable.Skip(startIndex.Value);

        if (takeCount != null)
            queryable = queryable.Take(takeCount.Value);

        return (await queryable.ToListAsync())
            .Select(mapper.Map<ClubDTO>).ToList();
    }
    
    public async Task<int> GetClubsCount(
        ClubFilter? filter = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Clubs // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                //queryable = queryable
                //    .Where(a => (a.FirstName.Contains(filter.Query)) ||
                //                (a.LastName.Contains(filter.Query)) ||
                //                (a.Email.Contains(filter.Query)) );
            }
        }

        return await queryable.CountAsync();
    }
    #endregion
    
    #region Coaches
    public async Task<IList<CoachDTO>> GetCoaches(
        CoachFilter? filter = null,
        int? startIndex = null, int? takeCount = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Coaches // clubs included automatically
                .OrderBy(a => a.LastName)
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.FirstName.Contains(filter.Query)) ||
                                (a.LastName.Contains(filter.Query)) ||
                                (a.Email.Contains(filter.Query)) );
            }
        }

        /* START & SKIP */
        if (startIndex != null)
            queryable = queryable.Skip(startIndex.Value);

        if (takeCount != null)
            queryable = queryable.Take(takeCount.Value);

        return (await queryable.ToListAsync())
            .Select(mapper.Map<CoachDTO>).ToList();
    }
    
    public async Task<int> GetCoachesCount(
        CoachFilter? filter = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Coaches // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.FirstName.Contains(filter.Query)) ||
                                (a.LastName.Contains(filter.Query)) ||
                                (a.Email.Contains(filter.Query)) );
            }
        }

        return await queryable.CountAsync();
    }
    #endregion
    
    #region Organizations
    public async Task<IList<OrganizationDTO>> GetOrganizations(
        OrganizationFilter? filter = null,
        int? startIndex = null, int? takeCount = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Organizations 
                .OrderBy(a => a.Name)
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.Name.Contains(filter.Query)) );
            }

            if (filter.Type != null)
            {
                queryable = queryable
                    .Where(a => a.Type == filter.Type);
            }
        }

        /* START & SKIP */
        if (startIndex != null)
            queryable = queryable.Skip(startIndex.Value);

        if (takeCount != null)
            queryable = queryable.Take(takeCount.Value);

        return (await queryable.ToListAsync())
            .Select(mapper.Map<OrganizationDTO>).ToList();
    }
    
    public async Task<int> GetOrganizationsCount(
        OrganizationFilter? filter = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Organizations // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .AsQueryable();

        /* FILTER */
        if (filter != null)
        {
            if (!String.IsNullOrEmpty(filter.Query))
            {
                queryable = queryable
                    .Where(a => (a.Name.Contains(filter.Query)) );
            }

            if (filter.Type != null)
            {
                queryable = queryable
                    .Where(a => a.Type == filter.Type);
            }
        }

        return await queryable.CountAsync();
    }
    #endregion

}