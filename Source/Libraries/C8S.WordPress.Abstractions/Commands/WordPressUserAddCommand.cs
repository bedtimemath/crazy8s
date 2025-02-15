using C8S.WordPress.Abstractions.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WordPressUserAddCommand: ICQRSCommand<DomainResponse<WordPressUserDetails>>
{
    public int PersonId { get; init; }
}