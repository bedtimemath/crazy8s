using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using C8S.Domain.Enums;
using SC.Common;
using SC.Common.Base;

namespace C8S.Domain.EFCore.Models;

[Table("Persons")]
public class PersonDb : BaseCoreDb
{
    #region Override Properties
    [NotMapped] 
    public override int Id => PersonId;
    [NotMapped] 
    public override string Display =>  String.Join(" ", [ FirstName, LastName ]) ;
    #endregion

    #region Id Property
    [Required] 
    public int PersonId { get; set; }
    #endregion

    #region Database Properties
    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string? FirstName { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Name)]
    public string LastName { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Email)]
    public string? Email { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? TimeZone { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? Phone { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public JobTitle? JobTitle { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.Medium)]
    public string? JobTitleOther { get; set; }

    public int? WordPressId { get; set; }
    #endregion

    #region Reference Properties
    [ForeignKey(nameof(Place))]
    public int? PlaceId { get; set; } = null!;
    public PlaceDb? Place { get; set; } = null!;
    #endregion

    #region Reference Collections
    public ICollection<PermissionDb> Permissions { get; set; } = null!;

    public ICollection<ClubPersonDb> ClubPersons { get; set; } = null!;
    public ICollection<TicketPersonDb> TicketPersons { get; set; } = null!;
    public ICollection<InvoicePersonDb> InvoicePersons { get; set; } = null!;
    public ICollection<PersonNoteDb> Notes { get; set; } = null!;
    #endregion

    #region Derived Properties
    [NotMapped]
    public string? FullName => String.IsNullOrEmpty(FirstName) ?
        LastName : $"{FirstName} {LastName}";
    #endregion
}