using C8S.Database.Abstractions.DTOs;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Application
    public async Task<ApplicationDTO> AddApplication(ApplicationDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<ApplicationDb>(dto);

        var entry = await dbContext.Applications.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<ApplicationDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<ApplicationDTO>> AddApplications(IEnumerable<ApplicationDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<ApplicationDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<ApplicationDb>(dto);
            var entry = await dbContext.Applications.AddAsync(db);
            dtosAdded.Add(mapper.Map<ApplicationDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    
    public async Task<ApplicationDTO> UpdateApplication(ApplicationDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<ApplicationDb>(dto);

        var entry = dbContext.Applications.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<ApplicationDTO>(entry.Entity);

        return dtoModified;
    }
    #endregion

    #region Coach
    public async Task<CoachDTO> AddCoach(CoachDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<CoachDb>(dto);

        var entry = await dbContext.Coaches.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<CoachDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<CoachDTO>> AddCoaches(IEnumerable<CoachDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<CoachDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<CoachDb>(dto);
            var entry = await dbContext.Coaches.AddAsync(db);
            dtosAdded.Add(mapper.Map<CoachDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    
    public async Task<CoachDTO> UpdateCoach(CoachDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<CoachDb>(dto);

        var entry = dbContext.Coaches.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<CoachDTO>(entry.Entity);

        return dtoModified;
    }
    #endregion

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