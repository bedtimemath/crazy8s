using C8S.Domain.Base;
using C8S.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.Domain.Obsolete.DTOs;

public class SkuDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => SkuId ?? 0;
    public override string Display => ClubKey;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.FulcoId)) errors.Add("FulcoId is required.");
        if (String.IsNullOrEmpty(this.Name)) errors.Add("Name is required.");
        if (this.Year == null) errors.Add("Year is required.");
        if (this.AgeLevel == null) errors.Add("AgeLevel is required.");
        if (this.Season == null) errors.Add("Season is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? SkuId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemSkuId { get; set; } = null;

    public string? FulcoId { get; set; } = null;

    public string? Name { get; set; } = null;

    public OfferStatus? Status { get; set; } = null;

    public string? Year { get; set; } = null;

    public int? Season { get; set; } = null;

    public AgeLevel? AgeLevel { get; set; } = null;

    public string? Version { get; set; } = null;

    public string? Notes { get; set; } = null;
    #endregion

    #region Parent Properties
    public ICollection<OrderDTO>? Orders { get; set; } = null;
    #endregion

    #region Derived Properties
    [NotMapped] 
    public string ClubKey => String.Join('-', [Year, Season, AgeLevel, Version]);
    #endregion
}