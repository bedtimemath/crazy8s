using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class PersonConfig : BaseCoreConfig<PersonDb>
{
    public override void Configure(EntityTypeBuilder<PersonDb> entity)
    {
        #region Id Property
        // [Required]
        // public int CoachId { get; set; }
        entity.HasKey(m => m.PersonId);
        #endregion

        #region Database Properties (Old System)
        //public Guid? OldSystemCoachId { get; set; } = null;
        entity.Property(m => m.OldSystemCoachId)
            .IsRequired(false);

        //public Guid? OldSystemOrganizationId { get; set; } = null;
        entity.Property(m => m.OldSystemOrganizationId)
            .IsRequired(false);

        //public Guid? OldSystemUserId { get; set; } = null;
        entity.Property(m => m.OldSystemUserId)
            .IsRequired(false);

        //public Guid? OldSystemCompanyId { get; set; } = null;
        entity.Property(m => m.OldSystemCompanyId)
            .IsRequired(false);
        #endregion

        #region Database Properties
        //[MaxLength(SharedConstants.MaxLengths.Name)]
        //public string? FirstName { get; set; }
        entity.Property(m => m.FirstName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Name)]
        //public string LastName { get; set; } = default!;
        entity.Property(m => m.LastName)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Name)
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Email)]
        //public string Email { get; set; }
        entity.Property(m => m.Email)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Email)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? TimeZone { get; set; }
        entity.Property(m => m.TimeZone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string? Phone { get; set; }
        entity.Property(m => m.Phone)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public JobTitle? JobTitle { get; set; }
        entity.Property(m => m.JobTitle)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(false);

        //[MaxLength(SoftCrowConstants.MaxLengths.Medium)]
        //public string? JobTitleOther { get; set; }
        entity.Property(m => m.JobTitleOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //public int? WordPressId { get; set; }
        entity.Property(m => m.WordPressId)
            .IsRequired(false);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Place))]
        //public int? PlaceId { get; set; } = default!;
        entity.Property(m => m.PlaceId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb? Place { get; set; } = default!;
        entity.HasOne(m => m.Place)
            .WithMany(m => m.Persons)
            .HasForeignKey(m => m.PlaceId)
            .IsRequired(false);

        //public ICollection<PermissionDb> Permissions { get; set; } = default!;
        entity.HasMany(m => m.Permissions)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public ICollection<RequestDb> Requests { get; set; } = default!;
        entity.HasMany(m => m.Requests)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public ICollection<ClubPersonDb> ClubPersons { get; set; } = default!;
        entity.HasMany(m => m.ClubPersons)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public ICollection<SalePersonDb> SalePersons { get; set; } = default!;
        entity.HasMany(m => m.SalePersons)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public ICollection<InvoicePersonDb> InvoicePersons { get; set; } = default!;
        entity.HasMany(m => m.InvoicePersons)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .IsRequired(false);

        //public ICollection<PersonNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Person)
            .HasForeignKey(m => m.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion

        #region Indices
        entity.HasIndex(m => m.Email)
            .IsUnique(true);
        entity.HasIndex(m => m.OldSystemCoachId)
            .IsUnique(true);
        #endregion
    }
}