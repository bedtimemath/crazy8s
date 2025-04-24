using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using SC.Common;

namespace SC.Audit.EFCore.Models;

[Table("DataChanges")]
public class DataChangeDb
{
    #region Id Property
    [Required] 
    public int DataChangeId { get; set; }
    #endregion

    #region Database Properties
    public Guid? Identifier { get; set; }
    
    public int? EntityId { get; set; }

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string EntityName { get; set; } = null!;

    [Required, MaxLength(SoftCrowConstants.MaxLengths.Short)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EntityState EntityState { get; set; }

    [MaxLength(SoftCrowConstants.MaxLengths.XXXLong)]
    public string? PropertiesJson { get; set; } = null!;

    [MaxLength(SoftCrowConstants.MaxLengths.Standard)]
    public string? Description { get; set; } = null!;

    [Required]
    public DateTimeOffset CreatedOn { get; set; }
    #endregion
}