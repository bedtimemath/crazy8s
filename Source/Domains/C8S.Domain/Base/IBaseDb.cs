namespace C8S.Domain.Base;

public interface IBaseDb: IAuditable
{
    int Id { get; }
    string Display { get; }
    Guid UniqueId { get; }
}