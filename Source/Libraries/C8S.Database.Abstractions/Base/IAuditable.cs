namespace C8S.Database.Abstractions.Base;

public interface IAuditable
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
}