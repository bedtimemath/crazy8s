using MediatR;

namespace C8S.AdminApp.Client.Services.Pages;

public abstract record PageCommand(
    string PageName,
    int? IdValue = null): IRequest
{
}

public record OpenPageCommand(string PageName, int? IdValue = null) : PageCommand(PageName, IdValue);
public record ClosePageCommand(string PageName, int? IdValue = null) : PageCommand(PageName, IdValue);