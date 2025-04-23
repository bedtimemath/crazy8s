using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.Domain.Features.Persons.Queries;

public record PersonTitleQuery : ICQRSQuery<WrappedResponse<string?>>
{
    public int PersonId { get; init; }
}