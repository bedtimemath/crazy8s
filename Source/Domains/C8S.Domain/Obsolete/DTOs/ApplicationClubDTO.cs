using C8S.Domain.Base;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Extensions;

namespace C8S.Domain.Obsolete.DTOs;

public class ApplicationClubDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => ApplicationClubId ?? 0;
    public override string Display => String.Join(" ", [ Season.ToString(), AgeLevel?.GetLabel() ]) 
                                      ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.AgeLevel == null) errors.Add("AgeLevel is required.");
        if (this.Season == null) errors.Add("Season is required.");
        if (this.StartsOn == null) errors.Add("StartsOn is required.");
        if (this.ApplicationId == null) errors.Add("ApplicationId is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? ApplicationClubId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemApplicationClubId { get; set; } = null;
    
    public Guid? OldSystemApplicationId { get; set; } = null;
    
    public Guid? OldSystemLinkedClubId { get; set; } = null;

    public AgeLevel? AgeLevel { get; set; } = null;

    public int? Season { get; set; } = null;

    public DateOnly? StartsOn { get; set; } = null;
    #endregion

    #region Parent Properties
    public int? ApplicationId { get; set; } = default!;
    #endregion
}