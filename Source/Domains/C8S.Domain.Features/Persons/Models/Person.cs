using System.Text.Json.Serialization;

namespace C8S.Domain.Features.Persons.Models;

public record Person
{
    public int PersonId { get; init; }

    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? FirstName { get; init; }

    [JsonIgnore]
    public string FullName =>
        string.Join(" ", new List<string?> { FirstName, LastName }
            .Where(s => !string.IsNullOrEmpty(s)));
}
