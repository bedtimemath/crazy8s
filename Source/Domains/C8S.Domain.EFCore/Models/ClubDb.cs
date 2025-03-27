using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Clubs")]
public class ClubDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubId;
    [NotMapped] 
    public override string Display => ClubKey;
    #endregion

    #region Id Property
    [Required] 
    public int ClubId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemClubId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemMeetingAddressId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubStatus Status { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string Year { get; set; } = null!;
    
    public int Season { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    public string? Version { get; set; } = null!;

    public DateOnly? StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Place))]
    public int PlaceId { get; set; }
    public PlaceDb Place { get; set; } = null!;

    [ForeignKey(nameof(Sale))]
    public int? SaleId { get; set; }
    public SaleDb? Sale { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ClubPersonDb> ClubPersons { get; set; } = null!;
    public ICollection<OrderDb> Orders { get; set; } = null!;
    public ICollection<ClubNoteDb> Notes { get; set; } = null!;
    #endregion

    #region Derived Properties
    [NotMapped] 
    public string ClubKey => String.Join('-', [Year, Season, AgeLevel, Version]);
    #endregion
}