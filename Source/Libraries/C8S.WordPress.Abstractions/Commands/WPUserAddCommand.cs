using C8S.WordPress.Abstractions.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.WordPress.Abstractions.Commands;

public record WPUserAddCommand: ICQRSCommand<WrappedResponse<WPUserDetails>>
{    
    public string Email { get; init; } = null!;

    public string? UserName { get; init; } = null!;
    public string? Name { get; init; } = null!;
    public string? Password { get; init; } = null!;

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public IList<string> Roles { get; init; } = [];
    public IDictionary<string, bool> Capabilities { get; init; } = new Dictionary<string, bool>();
}