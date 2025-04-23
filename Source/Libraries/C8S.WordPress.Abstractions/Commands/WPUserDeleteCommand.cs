using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserDeleteCommand: ICQRSCommand<WrappedResponse>
{
    public int Id { get; init; }
}