﻿using C8S.Domain.Base;
using C8S.Domain.Enums;
using SC.Common;

namespace C8S.Domain.Obsolete.DTOs;

public class OrganizationDTO: BaseDTO
{
    #region Property Overrides
    public override int Id => OrganizationId ?? 0;
    public override string Display => Name ?? SoftCrowConstants.Display.NotSet;
    #endregion

    #region Method Overrides
    public override IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();
        if (String.IsNullOrEmpty(this.Name)) errors.Add("Name is required.");
        if (String.IsNullOrEmpty(this.TimeZone)) errors.Add("TimeZone is required.");
        if (String.IsNullOrEmpty(this.Culture)) errors.Add("Culture is required.");
        if (this.Type == null) errors.Add("Type is required.");
        return errors;
    }
    #endregion

    #region Id Property
    public int? OrganizationId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemCompanyId { get; set; } = default!;
    
    public Guid? OldSystemOrganizationId { get; set; } = default!;
    
    public Guid? OldSystemPostalAddressId { get; set; } = default!;

    public string? Name { get; set; } = default!;

    public string? TimeZone { get; set; } = default!;

    public string? Culture { get; set; } = default!;

    public PlaceType? Type { get; set; } = PlaceType.Other;

    public string? TypeOther { get; set; } = null;

    public string? TaxIdentifier { get; set; } = null;

    public string? Notes { get; set; } = null;
    #endregion

    #region Reference Properties
    public int? AddressId { get; set; } = null;
    public AddressDTO? Address { get; set; } = null;
    #endregion

    #region Child Properties
    public ICollection<CoachDTO> Coaches { get; set; } = default!;
    public ICollection<ApplicationDTO> Applications { get; set; } = default!;
    #endregion
}