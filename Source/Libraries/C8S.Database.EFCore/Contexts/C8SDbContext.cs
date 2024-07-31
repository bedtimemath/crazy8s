#nullable disable
using C8S.Database.EFCore.Configs;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.EFCore.Contexts;

public class C8SDbContext(DbContextOptions<C8SDbContext> options) : DbContext(options)
{
    #region DbSet Properties
    public DbSet<AddressDb> Addresses { get; set; }
    public DbSet<ApplicationDb> Applications { get; set; }
    public DbSet<ApplicationClubDb> ApplicationClubs { get; set; }
    public DbSet<ClubDb> Clubs { get; set; }
    public DbSet<CoachDb> Coaches { get; set; }
    public DbSet<OrganizationDb> Organizations { get; set; }
    #endregion

    #region DbContext Overrides
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressConfig());
        modelBuilder.ApplyConfiguration(new ApplicationConfig());
        modelBuilder.ApplyConfiguration(new ApplicationClubConfig());
        modelBuilder.ApplyConfiguration(new ClubConfig());
        modelBuilder.ApplyConfiguration(new CoachConfig());
        modelBuilder.ApplyConfiguration(new OrganizationConfig());
    }
    #endregion
}
