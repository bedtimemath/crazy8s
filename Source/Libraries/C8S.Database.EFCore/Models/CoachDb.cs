using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Common;
using C8S.Database.Abstractions.Base;

namespace C8S.Database.EFCore.Models;

[Table("Coaches")]
public class CoachDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => CoachId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", new [] { FirstName, LastName }) 
                                       ?? SharedConstants.Display.NotSet;
    #endregion

    #region Id Property
    [Required] 
    public int CoachId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemUserId { get; set; } = null;

    public Guid? OldSystemCompanyId { get; set; } = null;

    [Required, MaxLength(SharedConstants.MaxLengths.Name)]
    public string FirstName { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Name)]
    public string LastName { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Email)]
    public string Email { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Medium)]
    public string TimeZone { get; set; } = default!;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? Phone { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Short)]
    public string? PhoneExt { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.XXXLong)]
    public string? OldSystemNotes { get; set; } = null;
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Organization))]
    public int? OrganizationId { get; set; } = default!;
    public OrganizationDb? Organization { get; set; } = default!;
    #endregion

    #region Reference Collections
    // one-to-one
    // one-to-many
    public ICollection<ApplicationDb> Applications { get; set; } = default!;

    // many-to-many
    #endregion
}