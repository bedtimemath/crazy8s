﻿using C8S.Domain.Base;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Extensions;

namespace C8S.Domain.Obsolete.DTOs;

public class SkuDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => SkuId ?? 0;
    public override string Display => String.Join(" ", new [] { Season.ToString(), AgeLevel?.GetLabel(), ClubSize?.GetLabel() }) 
                                      ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.Key)) errors.Add("Key is required.");
        if (String.IsNullOrEmpty(this.Name)) errors.Add("Name is required.");
        if (this.AgeLevel == null) errors.Add("AgeLevel is required.");
        if (this.ClubSize == null) errors.Add("ClubSize is required.");
        if (this.Season == null) errors.Add("Season is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? SkuId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemSkuId { get; set; } = null;

    public string? Key { get; set; } = null;

    public string? Name { get; set; } = null;

    public int? Season { get; set; } = null;

    public SkuStatus? Status { get; set; } = null;

    public AgeLevel? AgeLevel { get; set; } = null;

    public ClubSize? ClubSize { get; set; } = null;

    public string? Notes { get; set; } = null;
    #endregion

    #region Parent Properties
    public ICollection<OrderDTO>? Orders { get; set; } = null;
    #endregion
}