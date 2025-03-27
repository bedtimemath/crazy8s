using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class RequestConfig : BaseCoreConfig<RequestDb>
{
    public override void Configure(EntityTypeBuilder<RequestDb> entity)
    {
        #region Id Property
        // [Required]
        // public int RequestId { get; set; }
        entity.HasKey(m => m.RequestId);
        #endregion

        #region Database Properties
        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; }
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public long? FullSlateAppointmentId { get; set; }
        entity.Property(m => m.AppointmentId)
            .IsRequired(false);

        //public string? FullSlateAppointmentStarts { get; set; }
        entity.Property(m => m.AppointmentStartsOn)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Medium)]
        //public string? ReferenceSource { get; set; }
        entity.Property(m => m.ReferenceSource)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Long)]
        //public string? ReferenceSourceOther { get; set; }
        entity.Property(m => m.ReferenceSourceOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Long)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Comments)]
        //public string? Comments { get; set; }
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Comments)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public TicketDb? Ticket { get; set; }
        entity.HasOne(m => m.Ticket)
            .WithOne(m => m.Request)
            .HasForeignKey<TicketDb>(m => m.RequestId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}