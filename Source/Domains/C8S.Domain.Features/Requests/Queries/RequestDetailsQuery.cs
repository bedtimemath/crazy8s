using C8S.Domain.Features.Requests.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Requests.Queries;

public record RequestDetailsQuery : ICQRSQuery<DomainResponse<RequestDetails?>>
{
    public int RequestId { get; init; }
}