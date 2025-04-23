using C8S.Domain.Features.Persons.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Clubs.Queries;

public record PersonWithOrdersQuery : ICQRSQuery<WrappedResponse<PersonWithOrders?>>
{
    public int PersonId { get; init; }
}