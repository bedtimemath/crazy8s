using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class CoachSql
{
    #region Constants & ReadOnlys
    public const string SqlGet = 
        "SELECT c.[Id] AS [OldSystemCoachId], c.[OrganizationId] AS [OldSystemOrganizationId], u.[Id] AS [OldSystemUserId], u.[CompanyId] AS [OldSystemCompanyId], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes], c.[Created] AS [CreatedOn] FROM [Crazy8s].[Coach] c LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId] WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? CoachId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemUserId { get; set; } = null;

    public Guid? OldSystemCompanyId { get; set; } = null;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string TimeZone { get; set; } = default!;

    [NotMapped]
    public string PhoneString { get; set; } = default!;

    public string PhoneExt { get; set; } = default!;

    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Derived Properties
    public string? Phone => PhoneString.Length < 10 ? PhoneString :
        PhoneString switch
        {
            "0000000000" or null => null,
            _ => $"({PhoneString.Substring(0,3)}) {PhoneString.Substring(3,3)}-{PhoneString.Substring(6,4)}"
        };
    #endregion
}