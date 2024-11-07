using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Base;
using SC.Common;

namespace C8S.Domain.EFCore.Models;

[Table("Addresses")]
public class AddressDb : BaseDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => AddressId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", 
                                           (new [] { RecipientName, BusinessName, StreetAddress, City, State, PostalCode})
                                           .Where(s => !String.IsNullOrEmpty(s)));
    #endregion

    #region Id Property
    [Required] 
    public int AddressId { get; set; }
    #endregion

    #region Database Properties
    public Guid? OldSystemUsaPostalId { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? RecipientName { get; set; } = null;

    [MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? BusinessName { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string StreetAddress { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string City { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Tiny)]
    public string State { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.ZIPCode)]
    public string PostalCode { get; set; } = default!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string TimeZone { get; set; } = default!;

    [Required]
    public bool IsMilitary { get; set; } = default!;
    #endregion

    #region Parent Properties
    public ApplicationDb? Application { get; set; } = default!;
    public ClubDb? Club { get; set; } = default!;
    public OrderDb? Order { get; set; } = default!;
    public OrganizationDb? Organization { get; set; } = default!;
    #endregion
}