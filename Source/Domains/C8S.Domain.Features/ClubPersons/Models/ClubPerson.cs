using C8S.Domain.Features.Clubs.Models;
using C8S.Domain.Features.Persons.Models;

namespace C8S.Domain.Features.ClubPersons.Models;

public record ClubPerson
{
    public int ClubPersonId { get; set; }
    public bool IsPrimary { get; set; }
    public Person Person { get; set; } = null!;
    public Club Club { get; set; } = null!;
}