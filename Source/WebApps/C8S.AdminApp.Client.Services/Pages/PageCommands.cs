using MediatR;

namespace C8S.AdminApp.Client.Services.Pages;

public abstract record PageCommand(
    string PageUrlKey,
    int? IdValue = null): IRequest
{
}

public record OpenPageCommand(
    string PageUrlKey, 
    string PageTitle, 
    int? IdValue = null) : PageCommand(PageUrlKey, IdValue);
public record ClosePageCommand(
    string PageUrlKey, 
    int? IdValue = null) : PageCommand(PageUrlKey, IdValue);