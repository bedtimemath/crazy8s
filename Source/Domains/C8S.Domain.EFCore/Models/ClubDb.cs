﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;
using SC.Common.Extensions;

namespace C8S.Domain.EFCore.Models;

[Table("Clubs")]
public class ClubDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => ClubId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { Season.ToString(), AgeLevel.GetLabel(), ClubSize.GetLabel() }) 
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
    public int? Season { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AgeLevel? AgeLevel { get; set; } = default!;

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ClubSize? ClubSize { get; set; } = default!;

    public DateOnly? StartsOn { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Place))]
    public int PlaceId { get; set; } = default!;
    public PlaceDb Place { get; set; } = default!;
    #endregion

    #region Reference Collections
    public ICollection<OrderDb> Orders { get; set; } = default!;
    public ICollection<ClubPersonDb> ClubPersons { get; set; } = default!;
    #endregion
}