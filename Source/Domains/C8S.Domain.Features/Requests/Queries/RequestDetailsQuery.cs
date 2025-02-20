using C8S.Domain.Features.Requests.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestDetailsQuery : ICQRSQuery<WrappedResponse<RequestDetails?>>
{
    public int RequestId { get; init; }
}