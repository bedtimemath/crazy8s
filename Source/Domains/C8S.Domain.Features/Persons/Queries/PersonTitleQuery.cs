using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonTitleQuery : ICQRSQuery<DomainResponse<string?>>
{
    public int PersonId { get; init; }
}