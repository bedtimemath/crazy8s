using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Common;
using C8S.Common.Extensions;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.EFCore.Models;

[Table("Clubs")]
public class ClubDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { Season.ToString(), AgeLevel.GetLabel(), ClubSize.GetLabel() }) 
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int ClubId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemClubId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemMeetingAddressId { get; set; } = null;

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

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Coach))]
    public int CoachId { get; set; } = default!;
    public CoachDb Coach { get; set; } = default!;

    [ForeignKey(nameof(Organization))]
    public int OrganizationId { get; set; } = default!;
    public OrganizationDb Organization { get; set; } = default!;
    
    [ForeignKey(nameof(Address))]
    public int? AddressId { get; set; } = default!;
    public AddressDb? Address { get; set; } = default!;
    #endregion
}