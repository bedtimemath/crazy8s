using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserDeleteCommand: ICQRSCommand<WrappedResponse>
{
    public int Id { get; init; }
}