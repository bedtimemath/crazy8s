using C8S.WordPress.Abstractions.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

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
}