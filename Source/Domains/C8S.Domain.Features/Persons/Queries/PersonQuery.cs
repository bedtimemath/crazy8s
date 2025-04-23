using C8S.Domain.Features.Persons.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonQuery : ICQRSQuery<WrappedResponse<Person?>>
{
    public int PersonId { get; init; }
}