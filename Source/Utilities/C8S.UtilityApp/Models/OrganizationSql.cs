using System.ComponentModel.DataAnnotations.Schema;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.UtilityApp.Models;

public class OrganizationSql
{
    #region Id Property
    public int? OrganizationId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemCompanyId { get; set; } = default!;
    
    public Guid? OldSystemOrganizationId { get; set; } = default!;

    public string? Name { get; set; } = default!;

    public string? TimeZone { get; set; } = default!;

    public string? Culture { get; set; } = default!;

    [NotMapped]
    public string? OldSystemType { get; set; } = null;

    public string? TypeOther { get; set; } = null;

    public string? TaxIdentifier { get; set; } = null;

    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Derived Properties
    public OrganizationType Type =>
        OldSystemType switch
        {
            "Boys & Girls Club" => OrganizationType.BoysGirlsClub,
            "Home School Co-Op" => OrganizationType.HomeSchool,
            "Library" => OrganizationType.Library,
            "Other" => OrganizationType.Other,
            "School" => OrganizationType.School,
            "YMCA" => OrganizationType.YMCA,
            _ => throw new Exception("Unrecognized")
        };

    #endregion
}