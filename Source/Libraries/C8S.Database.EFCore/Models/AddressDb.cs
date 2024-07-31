using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using C8S.Common;
using C8S.Database.Abstractions.Base;

namespace C8S.Database.EFCore.Models;

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

    [MaxLength(SharedConstants.MaxLengths.Name)]
    public string? RecipientName { get; set; } = null;

    [MaxLength(SharedConstants.MaxLengths.Name)]
    public string? BusinessName { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Standard)]
    public string StreetAddress { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Medium)]
    public string City { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Tiny)]
    public string State { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.ZIPCode)]
    public string PostalCode { get; set; } = default!;

    [Required, MaxLength(SharedConstants.MaxLengths.Medium)]
    public string TimeZone { get; set; } = default!;

    [Required]
    public bool IsMilitary { get; set; } = default!;
    #endregion

    #region Parent Properties
    public OrganizationDb Organization { get; set; } = default!;
    #endregion
}