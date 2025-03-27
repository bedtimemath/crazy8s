using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;
using SC.Common.Extensions;

namespace C8S.Domain.EFCore.Models;

[Table("RequestedClubs")]
public class RequestedClubDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => RequestedClubId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", [ Season.ToString(), AgeLevel.GetLabel() ]);
    #endregion

    #region Id Property
    [Required] 
    public int RequestedClubId { get; set; }
    #endregion

    #region Database Properties (Old System)
    public Guid? OldSystemApplicationClubId { get; set; } = null;

    public Guid? OldSystemApplicationId { get; set; } = null;

    public Guid? OldSystemLinkedClubId { get; set; } = null;
    #endregion

    #region Database Properties
    [Required]
    public int Season { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; } = default!;

    [Required]
    public DateOnly StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Request))]
    public int RequestId { get; set; }
    public RequestDb Request { get; set; } = null!;
    #endregion
}