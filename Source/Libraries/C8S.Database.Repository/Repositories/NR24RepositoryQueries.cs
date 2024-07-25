using C8S.Database.Abstractions.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Coaches
    public async Task<IList<CoachDTO>> GetCoaches()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var queryable =
            dbContext.Coaches 
                .AsNoTracking()
                .AsQueryable();

        logger.LogDebug("Created queryable for Coaches");

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