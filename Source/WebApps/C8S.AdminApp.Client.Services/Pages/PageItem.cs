namespace C8S.AdminApp.Client.Services.Pages;

public record class PageItem
{
    public string Display { get; init; } = null!;
    public string Url { get; init; } = null!;
    public int? IdValue { get; init; }
}