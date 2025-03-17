using C8S.Domain.Features.Persons.Models;
using SC.Messaging.Abstractions.Base;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsListQuery : BaseListQuery<Person>
{
}