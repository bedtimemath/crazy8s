﻿using C8S.Domain.EFCore.Configs;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Interfaces;

namespace C8S.Domain.EFCore.Contexts;

public class C8SDbContext(
    ILoggerFactory loggerFactory,
    IServiceProvider serviceProvider,
    DbContextOptions<C8SDbContext> options) : DbContext(options)
{
    private static bool _auditWarned;

    private readonly ILogger<C8SDbContext> _logger = loggerFactory.CreateLogger<C8SDbContext>();
    private readonly IAuditInterceptor? _auditInterceptor = serviceProvider.GetService<IAuditInterceptor>();

    #region DbSet Properties
    public DbSet<ClubDb> Clubs { get; set; }
    public DbSet<PersonDb> Persons { get; set; }
    public DbSet<PlaceDb> Places { get; set; }
    public DbSet<ProposedClubDb> ProposedClubs { get; set; }
    public DbSet<RequestDb> Requests { get; set; }
    public DbSet<SkuDb> Skus { get; set; }
    public DbSet<UnfinishedDb> Unfinisheds { get; set; }
    public DbSet<WorkshopCodeDb> WorkshopCodes { get; set; }


    public DbSet<OrderDb> Orders { get; set; }
    public DbSet<OrderSkuDb> OrderSkus { get; set; }
    #endregion

    #region DbContext Overrides
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_auditInterceptor != null)
            optionsBuilder.AddInterceptors(_auditInterceptor);
        else if (!_auditWarned)
        {
            _logger.LogWarning("AuditInterceptor is not being used.");
            _auditWarned = true;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClubConfig());
        modelBuilder.ApplyConfiguration(new PersonConfig());
        modelBuilder.ApplyConfiguration(new PlaceConfig());
        modelBuilder.ApplyConfiguration(new ProposedClubConfig());
        modelBuilder.ApplyConfiguration(new RequestConfig());
        modelBuilder.ApplyConfiguration(new SkuConfig());
        modelBuilder.ApplyConfiguration(new UnfinishedConfig());
        modelBuilder.ApplyConfiguration(new WorkshopCodeConfig());


        modelBuilder.ApplyConfiguration(new OrderConfig());
        modelBuilder.ApplyConfiguration(new OrderSkuConfig());
    }
    #endregion
}
