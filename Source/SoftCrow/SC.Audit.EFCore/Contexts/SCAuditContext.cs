using Microsoft.EntityFrameworkCore;
using SC.Audit.EFCore.Configs;
using SC.Audit.EFCore.Models;

namespace SC.Audit.EFCore.Contexts;

public class SCAuditContext(DbContextOptions<SCAuditContext> options) : 
    DbContext(options)
{
    #region DbSet Properties
    public DbSet<DataChangeDb> DataChanges { get; set; }
    #endregion

    #region DbContext Overrides
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DataChangeConfig());
    }
    #endregion
}
