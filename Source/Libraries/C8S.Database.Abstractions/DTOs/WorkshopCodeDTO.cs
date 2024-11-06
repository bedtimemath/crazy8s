using C8S.Database.Abstractions.Base;

namespace C8S.Database.Abstractions.DTOs;

public class WorkshopCodeDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => WorkshopCodeId ?? 0;
    public override string Display => Key;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.Key)) errors.Add("Key is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? WorkshopCodeId { get; set; }
    #endregion

    #region Public Properties    
    public string? Key { get; set; } = null;

    public DateTimeOffset? StartsOn { get; set; } = null;

    public DateTimeOffset? EndsOn { get; set; } = null;
    #endregion
}