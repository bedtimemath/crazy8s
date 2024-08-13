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
}