using MediatR;

namespace C8S.AdminApp.Client.Services.Pages;

public abstract record PageCommand: IRequest
{
    public string PageUrl { get; init; } = null!;
    public int? IdValue { get; init; }
}

public record OpenPageCommand : PageCommand
{
    public string PageTitle { get; init; } = null!;
}
public record ClosePageCommand : PageCommand;