using C8S.AdminApp.Client.Services.Navigation.Enums;
using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Navigation.Models;

public record NavigationChange
{
    public string? OldUrl { get; init; } = null!;

    public NavigationAction Action { get; init; }

    public string PageUrl { get; init; } = null!;

    public DomainEntity? Entity { get; init; }

    public int? IdValue { get; init; }

    public string? JsonDetails { get; init; }
}