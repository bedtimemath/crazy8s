using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestTitleQuery : ICQRSQuery<DomainResponse<string?>>
{
    public int RequestId { get; init; }
}