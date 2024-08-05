using C8S.Database.Abstractions.Base;

namespace C8S.Database.Abstractions.DTOs;

public class AddressDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => AddressId ?? 0;
    public override string Display => String.Join(" ", 
        (new [] { RecipientName, BusinessName, StreetAddress, City, State, PostalCode})
        .Where(s => !String.IsNullOrEmpty(s)));
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.StreetAddress)) errors.Add("StreetAddress is required.");
        if (String.IsNullOrEmpty(this.City)) errors.Add("City is required.");
        if (String.IsNullOrEmpty(this.State)) errors.Add("State is required.");
        if (String.IsNullOrEmpty(this.PostalCode)) errors.Add("PostalCode is required.");
        if (String.IsNullOrEmpty(this.TimeZone)) errors.Add("TimeZone is required.");
        if (Organization == null) errors.Add("Organization is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? AddressId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemUsaPostalId { get; set; } = null;

    public string? RecipientName { get; set; } = null;

    public string? BusinessName { get; set; } = null;

    public string? StreetAddress { get; set; } = null;

    public string? City { get; set; } = null;

    public string? State { get; set; } = null;

    public string? PostalCode { get; set; } = null;

    public string? TimeZone { get; set; } = null;

    public bool IsMilitary { get; set; } = default(bool);
    #endregion

    #region Reference Properties
    public ApplicationDTO? Application { get; set; } = default!;
    public ClubDTO? Club { get; set; } = default!;
    public OrderDTO? Order { get; set; } = default!;
    public OrganizationDTO? Organization { get; set; } = null;
    #endregion
}