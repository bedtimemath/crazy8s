namespace C8S.Database.Abstractions.Base;

public interface IBaseDb: IAuditable
{
    int Id { get; }
    string Display { get; }
    Guid UniqueId { get; }
}