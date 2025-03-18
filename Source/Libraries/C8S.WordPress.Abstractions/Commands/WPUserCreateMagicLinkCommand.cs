using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserCreateMagicLinkCommand: ICQRSCommand<WrappedResponse<string>>
{    
    public int Id { get; init; }
}