﻿using C8S.Domain.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SC.Common;

namespace C8S.Domain.EFCore.Configs;

public class WorkshopCodeConfig : IEntityTypeConfiguration<WorkshopCodeDb>
{
    public void Configure(EntityTypeBuilder<WorkshopCodeDb> entity)
    {
        #region Id Property
        // [Required]
        // public int WorkshopCodeId { get; set; }
        entity.HasKey(m => m.WorkshopCodeId);
        #endregion

        #region Database Properties
        //[Required, MaxLength(SharedConstants.MaxLengths.Key)]
        //public string Key { get; set; } = default!;
        entity.Property(m => m.Key)
            .HasMaxLength(SoftCrowConstants.MaxLengths.Key)
            .IsRequired(true);

        //public DateTimeOffset? StartsOn { get; set; } = null;
        entity.Property(m => m.StartsOn)
            .IsRequired(false);

        //public DateTimeOffset? EndsOn { get; set; } = null;
        entity.Property(m => m.EndsOn)
            .IsRequired(false);
        #endregion
    }
}