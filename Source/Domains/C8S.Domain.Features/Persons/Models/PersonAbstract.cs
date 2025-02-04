using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Persons.Models;

public record PersonAbstract : PersonBase
{
    public string? FirstName { get; init; }

    [JsonIgnore]
    public string FullName =>
        string.Join(" ", new List<string?> { FirstName, LastName }
            .Where(s => !string.IsNullOrEmpty(s)));
}