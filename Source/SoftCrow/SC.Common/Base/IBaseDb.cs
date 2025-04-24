namespace SC.Common.Base;

public interface IBaseDb
{
    int Id { get; }
    string Display { get; }
    Guid? UniqueId { get; }
}