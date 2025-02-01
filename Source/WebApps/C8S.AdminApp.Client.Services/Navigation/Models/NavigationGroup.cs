using C8S.AdminApp.Client.Services.Navigation.Enums;

namespace C8S.AdminApp.Client.Services.Navigation.Models;

public record NavigationGroup
{
    public string Display { get; init; } = null!;
    public string IconString { get; init; } = null!;
    public NavigationEntity Entity { get; init; }
}