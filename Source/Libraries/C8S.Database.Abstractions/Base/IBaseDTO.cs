namespace C8S.Database.Abstractions.Base;

public interface IBaseDTO
{
    int Id { get; }
    string Display { get; }

    IEnumerable<string> GetValidationErrors();
}