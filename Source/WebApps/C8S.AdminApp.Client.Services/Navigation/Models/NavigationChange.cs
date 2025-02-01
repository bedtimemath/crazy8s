using C8S.AdminApp.Client.Services.Navigation.Enums;

namespace C8S.AdminApp.Client.Services.Navigation.Models;

public record NavigationChange
{
    public string CurrentUrl { get; init; } = null!;

    public NavigationAction Action { get; init; }

    public NavigationEntity Entity { get; init; }

    public string PageUrl { get; init; } = null!;

    public int? IdValue { get; init; }

    public string? JsonDetails { get; init; }
}