using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Menu.Models;

public record MenuItem
{
    public DomainEntity Entity { get; init; }
    public string Display { get; init; } = null!;
    public int IdValue { get; init; }
}