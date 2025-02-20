using C8S.WordPress.Abstractions.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserAddCommand: ICQRSCommand<WrappedResponse<WPUserDetails>>
{
    public int PersonId { get; init; }
}