namespace C8S.AdminApp.Client.Services.Navigation;

public record NavigationChange
{
    public string CurrentUrl { get; init; } = null!;

    public NavigationAction Action { get; init; }

    public NavigationGroup Group { get; init; }

    public string PageUrl { get; init; } = null!;

    public int? IdValue { get; init; }

    public string? JsonDetails { get; init; }
}