using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Menu.Models;

public record MenuGroup
{
    public DomainEntity? Entity { get; init; }
    public string Display { get; init; } = null!;
    public string IconString { get; init; } = null!;
    public string Url { get; init; } = null!;
}