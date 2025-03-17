using C8S.WordPress.Abstractions.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserUpdateRolesCommand: ICQRSCommand<WrappedResponse<WPUserDetails>>
{    
    public int Id { get; init; }
    public IEnumerable<string> Roles { get; init; } = [];
}