using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestTitleQuery : ICQRSQuery<WrappedResponse<string?>>
{
    public int RequestId { get; init; }
}