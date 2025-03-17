using C8S.WordPress.Abstractions.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserUpdateRolesCommand: ICQRSCommand<WrappedResponse<WPUserDetails>>
{    
    public string UserName { get; init; } = null!;
    public IList<string> Roles { get; init; } = [];
}