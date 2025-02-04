namespace C8S.Domain.Features.Persons.Models;

public abstract record PersonBase
{
    public int PersonId { get; init; }

    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
}