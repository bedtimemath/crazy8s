using C8S.AdminApp.Client.Services.Navigation.Enums;

namespace C8S.AdminApp.Client.Services.Navigation.Models;

public record NavigationChange
{
    public NavigationAction Action { get; init; }
    public string PageUrl { get; init; } = null!;
}