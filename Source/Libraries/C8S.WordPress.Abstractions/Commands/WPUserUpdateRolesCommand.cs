using C8S.WordPress.Abstractions.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserUpdateRolesCommand: ICQRSCommand<WrappedResponse<WPUserDetails>>
{    
    public int Id { get; init; }
    public IEnumerable<string> Roles { get; init; } = [];
}