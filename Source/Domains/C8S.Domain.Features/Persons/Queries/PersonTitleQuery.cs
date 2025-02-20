using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonTitleQuery : ICQRSQuery<WrappedResponse<string?>>
{
    public int PersonId { get; init; }
}