﻿namespace C8S.Domain.Base;

public abstract class BaseDTO: IBaseDTO
{
    #region Public Properties
    public abstract int Id { get; }
    public abstract string Display { get; }
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ModifiedOn { get; set; }
    #endregion

    #region Public Methods
    public abstract IEnumerable<string> GetValidationErrors();
    #endregion

    #region Public Overrides
    public override string ToString() => $"[{Id}] {Display}";

    // public override int GetHashCode() => Id.GetHashCode();
    // public override bool Equals(object? obj) => Equals(obj as BaseDb);
    // public bool Equals(BaseDb? other) => other != null && Id == other.Id;
    // public static bool operator ==(BaseDTO? left, BaseDTO? right) => Equals(left, right);
    // public static bool operator !=(BaseDTO? left, BaseDTO? right) => !Equals(left, right);
    #endregion
}