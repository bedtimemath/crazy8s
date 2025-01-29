using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Pages;

public abstract record PageCommand: ICQRSCommand
{
    public string PageUrl { get; init; } = null!;
    public int? IdValue { get; init; }
}

public record OpenPageCommand : PageCommand
{
    public string PageTitle { get; init; } = null!;
}
public record ClosePageCommand : PageCommand;