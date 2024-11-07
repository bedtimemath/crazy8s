using C8S.Domain.EFCore.Models;
using C8S.Domain.Obsolete.DTOs;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.Repository.Repositories;

public partial class C8SRepository
{
    #region Address
    public async Task<AddressDTO> AddAddress(AddressDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<AddressDb>(dto);

        var entry = await dbContext.Addresses.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<AddressDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<AddressDTO>> AddAddresses(IEnumerable<AddressDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<AddressDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<AddressDb>(dto);
            var entry = await dbContext.Addresses.AddAsync(db);
            dtosAdded.Add(mapper.Map<AddressDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    
    public async Task<AddressDTO> UpdateAddress(AddressDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<AddressDb>(dto);

        var entry = dbContext.Addresses.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<AddressDTO>(entry.Entity);

        return dtoModified;
    }
    #endregion

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

    #region Club
    public async Task<ClubDTO> AddClub(ClubDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<ClubDb>(dto);

        var entry = await dbContext.Clubs.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<ClubDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<ClubDTO>> AddClubs(IEnumerable<ClubDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<ClubDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<ClubDb>(dto);
            var entry = await dbContext.Clubs.AddAsync(db);
            dtosAdded.Add(mapper.Map<ClubDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    
    public async Task<ClubDTO> UpdateClub(ClubDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<ClubDb>(dto);

        var entry = dbContext.Clubs.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<ClubDTO>(entry.Entity);

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

    #region Order
    public async Task<OrderDTO> AddOrder(OrderDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<OrderDb>(dto);

        var entry = await dbContext.Orders.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<OrderDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<OrderDTO>> AddOrders(IEnumerable<OrderDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<OrderDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<OrderDb>(dto);
            var entry = await dbContext.Orders.AddAsync(db);
            dtosAdded.Add(mapper.Map<OrderDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    #endregion

    #region OrderSku
    public async Task<OrderSkuDTO> AddOrderSku(OrderSkuDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<OrderSkuDb>(dto);

        var entry = await dbContext.OrderSkus.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<OrderSkuDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<OrderSkuDTO>> AddOrderSkus(IEnumerable<OrderSkuDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<OrderSkuDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<OrderSkuDb>(dto);
            var entry = await dbContext.OrderSkus.AddAsync(db);
            dtosAdded.Add(mapper.Map<OrderSkuDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
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

    #region Sku
    public async Task<SkuDTO> AddSku(SkuDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<SkuDb>(dto);

        var entry = await dbContext.Skus.AddAsync(db);
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<SkuDTO>(entry.Entity);

        return dtoAdded;
    }

    public async Task<IEnumerable<SkuDTO>> AddSkus(IEnumerable<SkuDTO> dtos)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var dtosAdded = new List<SkuDTO>();
        foreach (var dto in dtos)
        {
            var db = mapper.Map<SkuDb>(dto);
            var entry = await dbContext.Skus.AddAsync(db);
            dtosAdded.Add(mapper.Map<SkuDTO>(entry.Entity));
        }

        await dbContext.SaveChangesAsync();
        return dtosAdded;
    }
    
    public async Task<SkuDTO> UpdateSku(SkuDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<SkuDb>(dto);

        var entry = dbContext.Skus.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<SkuDTO>(entry.Entity);

        return dtoModified;
    }
    #endregion

    #region Unfinished
    public async Task<UnfinishedDTO> AddUnfinished() =>
        await AddUnfinished(new UnfinishedDTO());
    public async Task<UnfinishedDTO> AddUnfinished(UnfinishedDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<UnfinishedDb>(dto);

        var entry = await dbContext.Unfinisheds.AddAsync(db);
        db.EndPart01On = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync();

        var dtoAdded = mapper.Map<UnfinishedDTO>(entry.Entity);

        return dtoAdded;
    }
    
    public async Task<UnfinishedDTO> UpdateUnfinished(UnfinishedDTO dto)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var db = mapper.Map<UnfinishedDb>(dto);

        var entry = dbContext.Unfinisheds.Attach(db);
        entry.State = EntityState.Modified;
        await dbContext.SaveChangesAsync();

        var dtoModified = mapper.Map<UnfinishedDTO>(entry.Entity);

        return dtoModified;
    }
    #endregion
}