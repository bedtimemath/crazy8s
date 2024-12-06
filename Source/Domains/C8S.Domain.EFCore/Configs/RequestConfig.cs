using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class RequestConfig : BaseConfig<RequestDb>
{
    public override void Configure(EntityTypeBuilder<RequestDb> entity)
    {
        #region Id Property
        // [Required]
        // public int ApplicationId { get; set; }
        entity.HasKey(m => m.ApplicationId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemApplicationId { get; set; } = null;
        entity.Property(m => m.OldSystemApplicationId)
            .IsRequired(false);

        //public Guid? OldSystemAddressId { get; set; } = null;
        entity.Property(m => m.OldSystemAddressId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedCoachId { get; set; } = null;
        entity.Property(m => m.OldSystemLinkedCoachId)
            .IsRequired(false);

        //public Guid? OldSystemLinkedOrganizationId { get; set; } = null;
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

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? PersonPhone { get; set; } = default!;
        entity.Property(m => m.PersonPhone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string PersonTimeZone { get; set; } = default!;
        entity.Property(m => m.PersonTimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string? OrganizationName { get; set; } = null;
        entity.Property(m => m.PlaceName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType? OrganizationType { get; set; } = null;
        entity.Property(m => m.PlaceType)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? OrganizationTypeOther { get; set; } = null;
        entity.Property(m => m.PlaceTypeOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? OrganizationTaxIdentifier { get; set; } = null;
        entity.Property(m => m.PlaceTaxIdentifier)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? WorkshopCode { get; set; } = null;
        entity.Property(m => m.WorkshopCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Medium)]
        //public string? ReferenceSource { get; set; } = null;
        entity.Property(m => m.ReferenceSource)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Long)]
        //public string? ReferenceSourceOther { get; set; } = null;
        entity.Property(m => m.ReferenceSourceOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Long)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.XXXLong)]
        //public string? Comments { get; set; } = null;
        entity.Property(m => m.Comments)
            .HasMaxLength(SoftCrowConstants.MaxLengths.XXXLong)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(LinkedCoach))]
        //public int? LinkedCoachId { get; set; } = default!;
        entity.Property(m => m.PersonId)
            .IsRequired(false);

        //[ForeignKey(nameof(LinkedOrganization))]
        //public int? LinkedOrganizationId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public CoachDb? LinkedCoach { get; set; } = default!;
        entity.HasOne(m => m.Person)
            .WithMany(m => m.Requests)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public OrganizationDb? LinkedOrganization { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Requests)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);
        #endregion

        #region Indices
        entity.HasIndex(m => m.OldSystemApplicationId)
            .IsUnique(true);
        #endregion
    }
}