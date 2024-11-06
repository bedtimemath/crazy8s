using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common.Extensions;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;
using SC.Common;

namespace C8S.Database.EFCore.Models;

[Table("ApplicationClubs")]
public class ApplicationClubDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ApplicationClubId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { Season.ToString(), AgeLevel.GetLabel(), ClubSize.GetLabel() }) 
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int ApplicationClubId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemApplicationClubId { get; set; } = null;

    public Guid? OldSystemApplicationId { get; set; } = null;

    public Guid? OldSystemLinkedClubId { get; set; } = null;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel AgeLevel { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize ClubSize { get; set; } = default!;

    [Required]
    public int Season { get; set; } = default!;

    [Required]
    public DateOnly StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Application))]
    public int ApplicationId { get; set; } = default!;
    public ApplicationDb Application { get; set; } = default!;
    #endregion
}