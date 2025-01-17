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

        #region Database Properties (Old System)
        //public Guid? OldSystemApplicationId { get; set; }
        entity.Property(m => m.OldSystemApplicationId)
            .IsRequired(false);

        //public Guid? OldSystemAddressId { get; set; }
        entity.Property(m => m.OldSystemAddressId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedCoachId { get; set; }
        entity.Property(m => m.OldSystemLinkedCoachId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedOrganizationId { get; set; }
        entity.Property(m => m.OldSystemLinkedOrganizationId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public ApplicationStatus Status { get; set; } = default!;
        entity.Property(m => m.Status)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
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

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string PersonLastName { get; set; } = default!;
        entity.Property(m => m.PersonLastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Email)]
        //public string PersonEmail { get; set; } = default!;
        entity.Property(m => m.PersonEmail)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? PersonPhone { get; set; } = default!;
        entity.Property(m => m.PersonPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string PersonTimeZone { get; set; } = default!;
        entity.Property(m => m.PersonTimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string? OrganizationName { get; set; }
        entity.Property(m => m.PlaceName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string? PlaceAddress1 { get; set; }
        entity.Property(m => m.PlaceAddress1)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string? PlaceAddress2 { get; set; }
        entity.Property(m => m.PlaceAddress2)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Medium)]
        //public string? PlaceCity { get; set; }
        entity.Property(m => m.PlaceCity)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
        //public string? PlaceState { get; set; }
        entity.Property(m => m.PlaceState)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
        //public string? PlacePostalCode { get; set; }
        entity.Property(m => m.PlacePostalCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.ZIPCode)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType? OrganizationType { get; set; }
        entity.Property(m => m.PlaceType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? OrganizationTypeOther { get; set; }
        entity.Property(m => m.PlaceTypeOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? OrganizationTaxIdentifier { get; set; }
        entity.Property(m => m.PlaceTaxIdentifier)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; }
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public long? FullSlateAppointmentId { get; set; }
        entity.Property(m => m.FullSlateAppointmentId)
            .IsRequired(false);

        //public string? FullSlateAppointmentStarts { get; set; }
        entity.Property(m => m.FullSlateAppointmentStartsOn)
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

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; }
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Person))]
        //public int? PersonId { get; set; }
        entity.Property(m => m.PersonId)
            .IsRequired(false);

        //[ForeignKey(nameof(Place))]
        //public int? PlaceId { get; set; }
        entity.Property(m => m.PlaceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PersonDb? Person { get; set; }
        entity.HasOne(m => m.Person)
            .WithMany(m => m.Requests)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public PlaceDb? Place { get; set; }
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Requests)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);

        //public SaleDb? Sale { get; set; }
        entity.HasOne(m => m.Sale)
            .WithOne(m => m.Request)
            .HasForeignKey<SaleDb>(m => m.RequestId)
            .IsRequired(false);

        //public ICollection<RequestedClubDb> RequestedClubs { get; set; } = default!;
        entity.HasMany(m => m.RequestedClubs)
            .WithOne(m => m.Request)
            .IsRequired(false);

        //public ICollection<RequestNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Request)
            .HasForeignKey(m => m.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemApplicationId)
            .IsUnique(true);
        #endregion
    }
}