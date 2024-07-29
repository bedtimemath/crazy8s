using C8S.Database.Abstractions.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Applications
    public async Task<IList<ApplicationDTO>> GetApplications(
        bool? whereLinkedCoach = null,
        bool? whereLinkedOrganization = null)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Applications // clubs included automatically
                .AsNoTracking()
                .AsQueryable();

        logger.LogDebug("Created queryable for Applications");

        /* LINKED COACH */
        if (whereLinkedCoach.HasValue)
        {
            if (whereLinkedCoach.Value)
            {
                queryable = queryable
                    .Where(a => a.LinkedCoachId != null);

                logger.LogDebug("Including only linked to coach");
            }
            else
            {
                queryable = queryable
                    .Where(a => a.LinkedCoachId == null);

                logger.LogDebug("Including only unlinked to coach");
            }
        }

        /* LINKED ORGANIZATION */
        if (whereLinkedOrganization.HasValue)
        {
            if (whereLinkedOrganization.Value)
            {
                queryable = queryable
                    .Where(a => a.LinkedOrganizationId != null);

                logger.LogDebug("Including only linked to organization");
            }
            else
            {
                queryable = queryable
                    .Where(a => a.LinkedOrganizationId == null);

                logger.LogDebug("Including only unlinked to organization");
            }
        }

        return (await queryable.ToListAsync())
            .Select(mapper.Map<ApplicationDTO>).ToList();
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
    public async Task<IList<OrganizationDTO>> GetOrganizations()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Organizations
                .AsNoTracking()
                .AsQueryable();

        logger.LogDebug("Created queryable for Organizations");

        return (await queryable.ToListAsync())
            .Select(mapper.Map<OrganizationDTO>).ToList();
    }
    #endregion
}