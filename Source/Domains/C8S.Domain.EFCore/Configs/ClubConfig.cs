using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class ClubConfig : BaseCoreConfig<ClubDb>
{
    public override void Configure(EntityTypeBuilder<ClubDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ClubId { get; set; }
        entity.HasKey(m => m.ClubId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ClubStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //public DateOnly? StartsOn { get; set; }
        entity.Property(m => m.StartsOn)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[Required, ForeignKey(nameof(Kit))]
        //public int KitId { get; set; } = default!;
        entity.Property(m => m.KitId)
            .IsRequired(true);
        
        //[Required, ForeignKey(nameof(Place))]
        //public int PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(true);

        //[ForeignKey(nameof(Ticket))]
        //public int? TicketId { get; set; } = default!;
        entity.Property(m => m.TicketId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public KitDb Kit { get; set; } = default!;
        entity.HasOne(m => m.Kit)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.KitId)
            .IsRequired(true);
        
        //public PlaceDb Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(true);

        //public TicketDb? Ticket { get; set; }
        entity.HasOne(m => m.Ticket)
            .WithMany(m => m.Clubs)
            .HasForeignKey(m => m.TicketId)
            .IsRequired(false);

        //public ICollection<ClubPersonDb> ClubPersons { get; set; } = default!;
        entity.HasMany(m => m.ClubPersons)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .IsRequired(false);

        //public ICollection<OrderDb> Orders { get; set; } = null!;
        entity.HasMany(m => m.Orders)
            .WithMany(m => m.Clubs)
            .UsingEntity("OrderClubs",
                l => l.HasOne(typeof(OrderDb)).WithMany()
                    .HasForeignKey(nameof(OrderDb.OrderId))
                    .HasPrincipalKey(nameof(OrderDb.OrderId)),
                r => r.HasOne(typeof(ClubDb)).WithMany()
                    .HasForeignKey(nameof(ClubDb.ClubId))
                    .HasPrincipalKey(nameof(ClubDb.ClubId)),
                j => j.HasKey(nameof(OrderDb.OrderId), nameof(ClubDb.ClubId)));

        //public ICollection<ClubNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Club)
            .HasForeignKey(m => m.ClubId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}