using C8S.Database.Abstractions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Application
    public async Task<ApplicationDTO> GetApplication(int applicationId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Applications // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        return mapper.Map<ApplicationDTO>(dto);
    }
    #endregion

    #region Coach
    public async Task<CoachDTO?> GetCoach(int coachId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Coaches // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(a => a.CoachId == coachId);

        return dto == null ? null : mapper.Map<CoachDTO>(dto);
    }
    public async Task<CoachDTO?> GetCoachByEmail(string email)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Coaches // clubs included automatically
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(c => c.Email == email);

        return dto == null ? null : mapper.Map<CoachDTO>(dto);
    }
    #endregion

    #region Organization
    public async Task<OrganizationDTO> GetOrganization(int organizationId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Organizations // clubs included automatically
            .AsNoTracking()
            .AsSingleQuery()
            .FirstOrDefaultAsync(a => a.OrganizationId == organizationId);

        return mapper.Map<OrganizationDTO>(dto);
    }
    #endregion

    #region Unfinished
    public async Task<UnfinishedDTO> GetUnfinished(int unfinishedId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Unfinisheds // clubs included automatically
            .AsNoTracking()
            .AsSingleQuery()
            .FirstOrDefaultAsync(a => a.UnfinishedId == unfinishedId);

        return mapper.Map<UnfinishedDTO>(dto);
    }

    public async Task<UnfinishedDTO> GetUnfinishedByCode(Guid code)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dto = await dbContext.Unfinisheds // clubs included automatically
            .AsNoTracking()
            .AsSingleQuery()
            .FirstOrDefaultAsync(a => a.Code == code);

        return mapper.Map<UnfinishedDTO>(dto);
    }
    #endregion
}