using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class UnfinishedConfig : BaseCoreConfig<UnfinishedDb>
{
    public override void Configure(EntityTypeBuilder<UnfinishedDb> entity)
    {
        #region Id Property
        // [Required]
        // public int UnfinishedId { get; set; }
        entity.HasKey(m => m.UnfinishedId);
        #endregion

        #region Database Properties
        //public Guid? Code { get; set; } = Guid.NewGuid();
        entity.Property(m => m.Code)
            .HasDefaultValueSql("NEWID()")
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public PersonType? PersonType { get; set; } = default!;
        entity.Property(m => m.PersonType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? PersonFirstName { get; set; } = default!;
        entity.Property(m => m.PersonFirstName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string PersonLastName { get; set; } = default!;
        entity.Property(m => m.PersonLastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Email)]
        //public string PersonEmail { get; set; } = default!;
        entity.Property(m => m.PersonEmail)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? PersonPhone { get; set; } = default!;
        entity.Property(m => m.PersonPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string PersonTimeZone { get; set; } = default!;
        entity.Property(m => m.PersonTimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public bool? HasHostedBefore { get; set; } = null;
        entity.Property(m => m.HasHostedBefore)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string? PlaceName { get; set; } = null;
        entity.Property(m => m.PlaceName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? PlaceAddress1 { get; set; } = default!;
        entity.Property(m => m.PlaceAddress1)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? PlaceAddress2 { get; set; } = default!;
        entity.Property(m => m.PlaceAddress2)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? PlaceCity { get; set; } = default!;
        entity.Property(m => m.PlaceCity)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Tiny)]
        //public string? State { get; set; } = default!;
        entity.Property(m => m.PlaceState)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.ZIPCode)]
        //public string? PostalCode { get; set; } = default!;
        entity.Property(m => m.PlacePostalCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.ZIPCode)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public PlaceType? PlaceType { get; set; } = null;
        entity.Property(m => m.PlaceType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? PlaceTypeOther { get; set; } = null;
        entity.Property(m => m.PlaceTypeOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? PlaceTaxIdentifier { get; set; } = null;
        entity.Property(m => m.PlaceTaxIdentifier)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public string? ClubsString { get; set; } = null;
        entity.Property(m => m.ClubsString)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XLong)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; } = null;
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public DateTimeOffset? ChosenTimeSlot { get; set; } = null;
        entity.Property(m => m.ChosenTimeSlot)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? ReferenceSource { get; set; } = null;
        entity.Property(m => m.ReferenceSource)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Long)]
        //public string? ReferenceSourceOther { get; set; } = null;
        entity.Property(m => m.ReferenceSourceOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Long)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; } = null;
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);

        //public DateTimeOffset? EndPart01On { get; set; }
        entity.Property(m => m.EndPart01On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart02On { get; set; }
        entity.Property(m => m.EndPart02On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart03On { get; set; }
        entity.Property(m => m.EndPart03On)
            .IsRequired(false);

        //public DateTimeOffset? EndPart04On { get; set; }
        entity.Property(m => m.EndPart04On)
            .IsRequired(false);

        //public DateTimeOffset? SubmittedOn { get; set; }
        entity.Property(m => m.SubmittedOn)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Request))]
        //public int? RequestId { get; set; } = default!;
        entity.Property(m => m.RequestId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Code)
            .IsUnique(true);
        #endregion
    }
}