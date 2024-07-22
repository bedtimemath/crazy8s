using C8S.Common;
using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.Database.Abstractions.DTOs;

public class CoachDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => CoachId ?? 0;
    public override string Display => Name ?? SharedConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.Name)) errors.Add("Name is required.");
        if (String.IsNullOrEmpty(this.Email)) errors.Add("Email is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? CoachId { get; set; }
    #endregion

    #region Public Properties    
    public string? Name { get; set; } = null;

    public string? Email { get; set; } = null;

    public string? Phone { get; set; } = null;

    public CoachStatus? Status { get; set; } = null;

    public string? Image { get; set; } = null;

    public string? TagLine { get; set; } = null;

    public string? Bio { get; set; } = null;

    public string? AuthId { get; set; } = null;
    #endregion

    #region Parent Properties
    public int? GroupId { get; set; } = null;
    #endregion

    #region Child Properties
    //public ICollection<LeadDTO> Leads { get; set; } = default!;
    #endregion
}