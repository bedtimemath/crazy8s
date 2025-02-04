using C8S.Domain.Features.Persons.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonDetailsQuery : ICQRSQuery<DomainResponse<PersonDetails?>>
{
    public int PersonId { get; init; }
}