using C8S.WordPress.Abstractions.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Queries;

public record WPUserByIdQuery: ICQRSQuery<WrappedResponse<WPUserDetails?>>
{
    public int Id { get; init; }
}