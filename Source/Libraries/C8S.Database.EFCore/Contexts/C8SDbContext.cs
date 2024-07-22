#nullable disable
using C8S.Database.EFCore.Configs;
using C8S.Database.EFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace C8S.Database.EFCore.Contexts;

public class C8SDbContext(DbContextOptions<C8SDbContext> options) : DbContext(options)
{
    #region DbSet Properties
    public DbSet<CoachDb> Coaches { get; set; }
    #endregion

    #region DbContext Overrides
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CoachConfig());
    }
    #endregion
}
