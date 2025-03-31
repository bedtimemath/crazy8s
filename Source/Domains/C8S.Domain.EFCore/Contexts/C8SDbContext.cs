using C8S.Domain.EFCore.Configs;
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
    public DbSet<ClubNoteDb> ClubNotes { get; set; }
    public DbSet<ClubPersonDb> ClubPersons { get; set; }
    public DbSet<InvoiceDb> Invoices { get; set; }
    public DbSet<InvoiceNoteDb> InvoiceNotes { get; set; }
    public DbSet<InvoicePersonDb> InvoicePersons { get; set; }
    public DbSet<KitDb> Kits { get; set; }
    public DbSet<KitPageDb> KitPages { get; set; }
    public DbSet<NoteDb> Notes { get; set; }
    public DbSet<OfferDb> Offers { get; set; }
    public DbSet<OrderDb> Orders { get; set; }
    public DbSet<OrderNoteDb> OrderNotes { get; set; }
    public DbSet<OrderOfferDb> OrderOffers { get; set; }
    public DbSet<PermissionDb> Permissions { get; set; }
    public DbSet<PersonDb> Persons { get; set; }
    public DbSet<PersonNoteDb> PersonNotes { get; set; }
    public DbSet<PlaceDb> Places { get; set; }
    public DbSet<PlaceNoteDb> PlaceNotes { get; set; }
    public DbSet<RequestDb> Requests { get; set; }
    public DbSet<TicketDb> Tickets { get; set; }
    public DbSet<TicketNoteDb> TicketNotes { get; set; }
    public DbSet<TicketPersonDb> TicketPersons { get; set; }
    public DbSet<ShipmentDb> Shipments { get; set; }
    public DbSet<UnfinishedDb> Unfinisheds { get; set; }
    public DbSet<WorkshopCodeDb> WorkshopCodes { get; set; }

    public DbSet<OldNewDb> OldNews { get; set; }
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
        modelBuilder.ApplyConfiguration(new ClubNoteConfig());
        modelBuilder.ApplyConfiguration(new ClubPersonConfig());
        modelBuilder.ApplyConfiguration(new InvoiceConfig());
        modelBuilder.ApplyConfiguration(new InvoiceNoteConfig());
        modelBuilder.ApplyConfiguration(new InvoicePersonConfig());
        modelBuilder.ApplyConfiguration(new KitConfig());
        modelBuilder.ApplyConfiguration(new KitPageConfig());
        modelBuilder.ApplyConfiguration(new NoteConfig());
        modelBuilder.ApplyConfiguration(new OfferConfig());
        modelBuilder.ApplyConfiguration(new OrderConfig());
        modelBuilder.ApplyConfiguration(new OrderNoteConfig());
        modelBuilder.ApplyConfiguration(new OrderOfferConfig());
        modelBuilder.ApplyConfiguration(new PermissionConfig());
        modelBuilder.ApplyConfiguration(new PersonConfig());
        modelBuilder.ApplyConfiguration(new PersonNoteConfig());
        modelBuilder.ApplyConfiguration(new PlaceConfig());
        modelBuilder.ApplyConfiguration(new PlaceNoteConfig());
        modelBuilder.ApplyConfiguration(new RequestConfig());
        modelBuilder.ApplyConfiguration(new TicketConfig());
        modelBuilder.ApplyConfiguration(new TicketNoteConfig());
        modelBuilder.ApplyConfiguration(new TicketPersonConfig());
        modelBuilder.ApplyConfiguration(new ShipmentConfig());
        modelBuilder.ApplyConfiguration(new UnfinishedConfig());
        modelBuilder.ApplyConfiguration(new WorkshopCodeConfig());

        modelBuilder.ApplyConfiguration(new OldNewConfig());
    }
    #endregion
}
