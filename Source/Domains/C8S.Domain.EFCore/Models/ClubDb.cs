using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;
using SC.Common.Extensions;

namespace C8S.Domain.EFCore.Models;

[Table("Clubs")]
public class ClubDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubId;
    [NotMapped] 
    public override string Display =>  String.Join("-", 
                                           [ 
                                               Season?.ToString() ?? SoftCrowConstants.Display.NotSet, 
                                               AgeLevel?.GetLabel() ?? SoftCrowConstants.Display.NotSet, 
                                               ClubSize?.GetLabel() ?? SoftCrowConstants.Display.NotSet 
                                           ]) 
                                       ?? SoftCrowConstants.Display.NotSet;
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
    
    public int? Season { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel? AgeLevel { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize? ClubSize { get; set; }

    public DateOnly? StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [Required, ForeignKey(nameof(Place))]
    public int PlaceId { get; set; } = default!;
    public PlaceDb Place { get; set; } = default!;

    [ForeignKey(nameof(Sale))]
    public int? SaleId { get; set; }
    public SaleDb? Sale { get; set; }

    // one-to-one
    public OrderDb? Order { get; set; }
    #endregion

    #region Reference Collections
    public ICollection<ClubPersonDb> ClubPersons { get; set; } = default!;
    public ICollection<ClubNoteDb> Notes { get; set; } = default!;
    #endregion
}