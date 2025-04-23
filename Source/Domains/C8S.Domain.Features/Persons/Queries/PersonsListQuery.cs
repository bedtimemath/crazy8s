using C8S.Domain.Features.Persons.Models;
using SC.Common.Helpers.CQRS.Base;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonsListQuery : BaseListQuery<Person>
{
}