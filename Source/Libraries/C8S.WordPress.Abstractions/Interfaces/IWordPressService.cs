using C8S.Domain.Features.Persons.Models;

namespace C8S.WordPress.Abstractions.Interfaces;

public interface IWordPressService
{
    Task CreateWordPressUserFromPerson(PersonDetails person);
}