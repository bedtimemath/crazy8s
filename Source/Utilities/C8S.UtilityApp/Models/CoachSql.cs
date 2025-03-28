using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class CoachSql
{
    #region Constants & ReadOnlys
    public const string SqlGet = 
        "SELECT c.[Id] AS [OldSystemCoachId], c.[OrganizationId] AS [OldSystemOrganizationId], u.[Id] AS [OldSystemUserId], u.[CompanyId] AS [OldSystemCompanyId], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Role], c.[Notes], CAST(c.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Coach] c  LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId]  WHERE c.[DeletedBy] IS NULL";
    public const string SqlGetDeleted = 
        "SELECT c.[Id] AS [OldSystemCoachId], c.[OrganizationId] AS [OldSystemOrganizationId], u.[Id] AS [OldSystemUserId], u.[CompanyId] AS [OldSystemCompanyId], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Role], c.[Notes], CAST(c.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Coach] c  LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId]  WHERE c.[Id] = @Id";
    #endregion

    #region Id Property
    public int? CoachId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemUserId { get; set; } = null;

    public Guid? OldSystemCompanyId { get; set; } = null;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    [NotMapped]
    public string PhoneString { get; set; } = null!;

    public string? PhoneExt { get; set; } = null!;

    public string? Role { get; set; } = null!;

    public string? Notes { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public string? Phone => PhoneString.Length < 10 ? PhoneString :
        PhoneString switch
        {
            "0000000000" or null => null,
            _ => $"({PhoneString.Substring(0,3)}) {PhoneString.Substring(3,3)}-{PhoneString.Substring(6,4)}"
        };

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}