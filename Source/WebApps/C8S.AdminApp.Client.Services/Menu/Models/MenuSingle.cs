namespace C8S.AdminApp.Client.Services.Menu.Models;

public record MenuSingle
{
    public string Display { get; init; } = null!;
    public string IconString { get; init; } = null!;
    public string Url { get; init; } = null!;
}