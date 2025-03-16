using C8S.Domain.Features.ClubPersons.Models;

namespace C8S.Domain.Features.Persons.Models;

public record PersonWithOrders : Person
{
    public List<ClubPersonWithOrders> ClubPersons { get; init; } = [];
}
