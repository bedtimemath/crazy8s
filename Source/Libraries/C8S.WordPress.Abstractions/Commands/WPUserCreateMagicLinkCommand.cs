using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserCreateMagicLinkCommand: ICQRSCommand<WrappedResponse<string>>
{    
    public int Id { get; init; }
}