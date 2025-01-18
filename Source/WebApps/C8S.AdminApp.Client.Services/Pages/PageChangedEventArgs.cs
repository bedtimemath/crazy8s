namespace C8S.AdminApp.Client.Services.Pages;

public class PageChangedEventArgs: EventArgs
{
    public PageChangedAction Action { get; set; }
    public string OldUrl { get; set; } = null!;
    public string NewUrl { get; set; } = null!;
}