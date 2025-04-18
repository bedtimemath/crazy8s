﻿using C8S.Domain.EFCore.Base;
using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class PlaceConfig : BaseCoreConfig<PlaceDb>
{
    public override void Configure(EntityTypeBuilder<PlaceDb> entity)
    {
        #region Id Property
        // [Required]
        // public int OrganizationId { get; set; }
        entity.HasKey(m => m.PlaceId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SharedConstants.MaxLengths.FullName)]
        //public string Name { get; set; } = default!;
        entity.Property(m => m.Name)
            .HasMaxLength(SoftCrowConstants.MaxLengths.FullName)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Short)]
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        //public OrganizationType Type { get; set; } = OrganizationType.Other;
        entity.Property(m => m.Type)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .HasConversion<string>()
            .IsRequired(true);

        //[MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string? TypeOther { get; set; }
        entity.Property(m => m.TypeOther)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(false);

        //[MaxLength(SharedConstants.MaxLengths.Short)]
        //public string? TaxIdentifier { get; set; }
        entity.Property(m => m.TaxIdentifier)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Short)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Standard)]
        //public string StreetAddress { get; set; } = default!;
        entity.Property(m => m.Line1)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(true);

        //[MaxLength(SoftCrowConstants.MaxLengths.Standard)]
        //public string? Line2 { get; set; }
        entity.Property(m => m.Line2)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Standard)
            .IsRequired(false);

        //[Required, MaxLength(SharedConstants.MaxLengths.Medium)]
        //public string City { get; set; } = default!;
        entity.Property(m => m.City)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Medium)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.Tiny)]
        //public string State { get; set; } = default!;
        entity.Property(m => m.State)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Tiny)
            .IsRequired(true);

        //[Required, MaxLength(SharedConstants.MaxLengths.ZIPCode)]
        //public string PostalCode { get; set; } = default!;
        entity.Property(m => m.ZIPCode)
            .HasMaxLength(SoftCrowConstants.MaxLengths.ZIPCode)
            .IsRequired(true);

        //[Required]
        //public bool IsMilitary { get; set; } = default!;
        entity.Property(m => m.IsMilitary)
            .HasDefaultValue(false)
            .IsRequired(true);
        #endregion

        #region Reference Properties
        //[ForeignKey(nameof(Parent))]
        //public int? ParentId { get; set; } = default!;
        entity.Property(m => m.ParentId)
            .IsRequired(false);
        #endregion

        #region Navigation Configuration
        //public PlaceDb? Parent { get; set; } = default!;
        entity.HasOne(m => m.Parent)
            .WithMany(m => m.Children)
            .IsRequired(false);

        //public ICollection<PlaceDb> Children { get; set; } = default!;
        entity.HasMany(m => m.Children)
            .WithOne(m => m.Parent)
            .HasForeignKey(m => m.ParentId)
            .IsRequired(false);

        //public ICollection<ClubDb> Clubs { get; set; } = default!;
        entity.HasMany(m => m.Clubs)
            .WithOne(m => m.Place)
            .HasForeignKey(m => m.PlaceId);

        //public ICollection<PersonDb> Persons { get; set; } = default!;
        entity.HasMany(m => m.Persons)
            .WithOne(m => m.Place)
            .HasForeignKey(m => m.PlaceId);

        //public ICollection<TicketDb> Tickets { get; set; } = default!;
        entity.HasMany(m => m.Tickets)
            .WithOne(m => m.Place)
            .HasForeignKey(m => m.PlaceId);

        //public ICollection<PlaceNoteDb> Notes { get; set; } = default!;
        entity.HasMany(m => m.Notes)
            .WithOne(m => m.Place)
            .HasForeignKey(m => m.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
        #endregion
    }
}