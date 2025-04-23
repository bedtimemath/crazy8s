using C8S.WordPress.Abstractions.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.WordPress.Abstractions.Queries;

public record WPUserByIdQuery: ICQRSQuery<WrappedResponse<WPUserDetails?>>
{
    public int Id { get; init; }
}