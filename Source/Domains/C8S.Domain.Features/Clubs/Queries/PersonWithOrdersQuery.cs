using C8S.Domain.Features.Persons.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.Domain.Features.Clubs.Queries;

public record PersonWithOrdersQuery : ICQRSQuery<WrappedResponse<PersonWithOrders?>>
{
    public int PersonId { get; init; }
}