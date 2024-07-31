using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Organizations")]
public class OrganizationDb: BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => OrganizationId;
    [NotMapped] 
    public override string Display => Name;
    #endregion

    #region Id Property
    [Required] 
    public int OrganizationId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemCompanyId { get; set; } = null;
    
    public Guid? OldSystemOrganizationId { get; set; } = null;
    
    public Guid? OldSystemPostalAddressId { get; set; } = null;

    [Required, MaxLength(SharedConstants.MaxLengths.FullName)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Medium)]
    public string TimeZone { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    public string Culture { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrganizationType Type { get; set; } = OrganizationType.Other;

    [MaxLength(SharedConstants.MaxLengths.Medium)]
    public string? TypeOther { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? TaxIdentifier { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Address))]
    public int? AddressId { get; set; } = default!;
    public AddressDb? Address { get; set; } = default!;
    #endregion

    #region Child Properties
    // one-to-many
    public ICollection<ClubDb> Clubs { get; set; } = default!;
    public ICollection<CoachDb> Coaches { get; set; } = default!;
    public ICollection<ApplicationDb> Applications { get; set; } = default!;
    #endregion
}