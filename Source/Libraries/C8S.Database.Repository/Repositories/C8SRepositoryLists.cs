using C8S.Database.Abstractions.DTOs;
using C8S.Database.Abstractions.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
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

        logger.LogDebug("Created queryable for Applications");

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

        logger.LogDebug("Created queryable for Applications");

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

        logger.LogDebug("Created queryable for ApplicationClubs");

        return (await queryable.ToListAsync())
            .Select(mapper.Map<ApplicationClubDTO>).ToList();
    }
    #endregion

    #region Coaches
    public async Task<IList<CoachDTO>> GetCoaches(
        bool? whereLinkedOrganization = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Coaches
                .AsNoTracking()
                .AsQueryable();

        logger.LogDebug("Created queryable for Coaches");

        /* LINKED ORGANIZATION */
        if (whereLinkedOrganization.HasValue)
        {
            if (whereLinkedOrganization.Value)
            {
                queryable = queryable
                    .Where(c => c.OrganizationId != null);

                logger.LogDebug("Including only linked to organization");
            }
            else
            {
                queryable = queryable
                    .Where(c => c.OrganizationId == null);

                logger.LogDebug("Including only unlinked to organization");
            }
        }

        return (await queryable.ToListAsync())
            .Select(mapper.Map<CoachDTO>).ToList();
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

        logger.LogDebug("Created queryable for Organizations");

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

        logger.LogDebug("Created queryable for Organizations");

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