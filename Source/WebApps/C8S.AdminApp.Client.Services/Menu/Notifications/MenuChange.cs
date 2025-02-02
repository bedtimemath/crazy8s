using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Menu.Notifications;

public record MenuChange
{
    public DomainEntity Entity { get; init; }
}