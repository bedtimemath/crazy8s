using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Base;
using C8S.Domain.Enums;
using SC.Common;

namespace C8S.Domain.Obsolete.DTOs;

public class ClubDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => ClubId ?? 0;
    public override string Display => ClubKey;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (this.AgeLevel == null) errors.Add("AgeLevel is required.");
        if (this.Season == null) errors.Add("Season is required.");
        if (this.StartsOn == null) errors.Add("StartsOn is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? ClubId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemClubId { get; set; } = null;
    
    public Guid? OldSystemOrganizationId { get; set; } = null;
    
    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemMeetingAddressId { get; set; } = null;

    public string? Year { get; set; } = null;

    public int? Season { get; set; } = null;

    public AgeLevel? AgeLevel { get; set; } = null;

    public string? Version { get; set; } = null;

    public DateOnly? StartsOn { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    public int? CoachId { get; set; } = default!;
    public CoachDTO? Coach { get; set; } = default!;

    public int? OrganizationId { get; set; } = default!;
    public OrganizationDTO? Organization { get; set; } = default!;
    
    public int? AddressId { get; set; } = default!;
    public AddressDTO? Address { get; set; } = default!;
    #endregion

    #region Parent Properties
    public ICollection<OrderDTO>? Orders { get; set; } = null;
    #endregion

    #region Derived Properties
    [NotMapped] 
    public string ClubKey => String.Join('-', [Year, Season, AgeLevel, Version]);
    #endregion
}