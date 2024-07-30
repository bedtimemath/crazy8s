using C8S.Common;
using C8S.Database.Abstractions.Base;

namespace C8S.Database.Abstractions.DTOs;

public class CoachDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => CoachId ?? 0;
    public override string Display => String.Join(" ", new [] { FirstName, LastName }) 
                                      ?? SharedConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.LastName)) errors.Add("LastName is required.");
        if (String.IsNullOrEmpty(this.Email)) errors.Add("Email is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? CoachId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemUserId { get; set; } = null;

    public Guid? OldSystemCompanyId { get; set; } = null;

    public string? FirstName { get; set; } = null;

    public string? LastName { get; set; } = null;

    public string? Email { get; set; } = null;

    public string? TimeZone { get; set; } = null;

    public string? Phone { get; set; } = null;

    public string? PhoneExt { get; set; } = null;

    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Parent Properties
    public int? OrganizationId { get; set; } = null;
    #endregion

    #region Child Properties
    //public ICollection<LeadDTO> Leads { get; set; } = default!;
    #endregion

    #region Derived Properties
    public string? FullName => 
        String.IsNullOrEmpty(FirstName) && String.IsNullOrEmpty(LastName) ? null : 
            String.Join(" ", new List<string?>() { FirstName, LastName });
    
    public string? FullPhone => 
        String.IsNullOrEmpty(Phone) && String.IsNullOrEmpty(PhoneExt) ? null : 
            String.Join(" x", new List<string?>() { Phone, PhoneExt });

    #endregion
}