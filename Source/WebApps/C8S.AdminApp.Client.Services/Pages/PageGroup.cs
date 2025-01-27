namespace C8S.AdminApp.Client.Services.Pages;

public record PageGroup
{
    public string Icon { get; init; } = null!;
    public string Display { get; init; } = null!;
    public string Url { get; init; } = null!;
}