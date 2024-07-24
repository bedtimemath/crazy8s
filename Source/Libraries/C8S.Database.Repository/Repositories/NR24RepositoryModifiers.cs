using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Models;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Organization

    public async Task<OrganizationDTO> AddOrganization(OrganizationDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<OrganizationDb>(dto);

        var entry = await dbContext.Organizations.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<OrganizationDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<OrganizationDTO>> AddOrganizations(IEnumerable<OrganizationDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<OrganizationDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<OrganizationDb>(dto);
            var entry = await dbContext.Organizations.AddAsync(db);
            dtosAdded.Add(mapper.Map<OrganizationDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    #endregion
}