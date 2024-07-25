using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Common;

namespace C8S.UtilityApp.Models;

public class CoachSql
{
    #region Id Property
    public int? CoachId { get; set; }
    #endregion

    #region Public Properties    
    [NotMapped]
    public string? OldSystemCoachIdString { get; set; } = null;

    [NotMapped]
    public string? OldSystemOrganizationIdString { get; set; } = null;

    [NotMapped]
    public string? OldSystemUserIdString { get; set; } = null;

    [NotMapped]
    public string? OldSystemCompanyIdString { get; set; } = null;

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
    public Guid? OldSystemCoachId => String.IsNullOrEmpty(OldSystemCoachIdString) ? null :
        new Guid(OldSystemCoachIdString);

    public Guid? OldSystemOrganizationId => String.IsNullOrEmpty(OldSystemOrganizationIdString) ? null :
        new Guid(OldSystemOrganizationIdString);

    public Guid? OldSystemUserId => String.IsNullOrEmpty(OldSystemUserIdString) ? null :
        new Guid(OldSystemUserIdString);

    public Guid? OldSystemCompanyId => String.IsNullOrEmpty(OldSystemCompanyIdString) ? null :
        new Guid(OldSystemCompanyIdString);

    public string? Phone => PhoneString.Length < 10 ? PhoneString :
        PhoneString switch
        {
            "0000000000" or null => null,
            _ => $"({PhoneString.Substring(0,3)}) {PhoneString.Substring(3,3)}-{PhoneString.Substring(6,4)}"
        };

    #endregion
}