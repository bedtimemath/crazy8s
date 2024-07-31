using System.ComponentModel.DataAnnotations.Schema;
using C8S.Database.Abstractions.Enumerations;

namespace C8S.UtilityApp.Models;

public class OrganizationSql
{
    #region Constants & ReadOnlys
    public const string SqlGet = 
        "SELECT c.[Id] AS [OldSystemCompanyId], o.[Id] AS [OldSystemOrganizationId], o.[PostalAddressId] AS [OldSystemPostalAddressId], c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture], t.[Name] AS [OldSystemType], o.[OrganizationTypeOther] AS [TypeOther], CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier], o.[Notes] As [OldSystemNotes], CAST(o.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Organization] o LEFT JOIN [Crazy8s].[OrganizationType] t ON t.[Id] = o.[OrganizationTypeId] LEFT JOIN [Bits].[Company] c ON o.[CompanyId] = c.[Id] WHERE c.[DeletedBy] IS NULL AND o.[DeletedBy] IS NULL";
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

    [NotMapped]
    public string? OldSystemType { get; set; } = null;

    public string? TypeOther { get; set; } = null;

    public string? TaxIdentifier { get; set; } = null;

    public string? OldSystemNotes { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
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

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}