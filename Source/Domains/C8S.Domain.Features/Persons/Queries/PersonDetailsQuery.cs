using C8S.Domain.Features.Persons.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonDetailsQuery : ICQRSQuery<WrappedResponse<PersonDetails?>>
{
    public int PersonId { get; init; }
}