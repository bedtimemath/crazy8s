namespace C8S.Domain.Base;

public interface IBaseDTO
{
    int Id { get; }
    string Display { get; }
    DateTimeOffset CreatedOn { get; set; }

    IEnumerable<string> GetValidationErrors();
}