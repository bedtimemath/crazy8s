using C8S.WordPress.Abstractions.Models;

namespace C8S.WordPress.Abstractions.Notifications;

public record WPUsersUpdated
{
    public WPUserDetails WPUser { get; init; } = null!;
}