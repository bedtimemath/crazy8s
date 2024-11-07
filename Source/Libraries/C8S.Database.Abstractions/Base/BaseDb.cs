using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.Database.Abstractions.Base;

/// <summary>
///		Abstract base class for any dbContext model. Requires that the model have
/// an Id field (provides unique identifier) and a Display field (provides a
/// human-readable, hopefully-unique identifier). It also combines the two for
/// the ToString method so derived class don't need to do the same. 
/// </summary>
public abstract class BaseDb: IBaseDb
{
    #region Public Properties
    [NotMapped]
    public abstract int Id { get; }
    [NotMapped]
    public abstract string Display { get; }
    [NotMapped]
    public virtual Guid UniqueId { get; } = Guid.NewGuid();

    [Required]
    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
    #endregion

    #region Public Overrides
    public override string ToString() => $"[{Id}] {Display}";

    public override int GetHashCode() => Id.GetHashCode();
    public override bool Equals(object? obj) => Equals(obj as BaseDb);
    public bool Equals(BaseDb? other) => other != null && Id == other.Id;
    public static bool operator ==(BaseDb? left, BaseDb? right) => Equals(left, right);
    public static bool operator !=(BaseDb? left, BaseDb? right) => !Equals(left, right);
    #endregion
}