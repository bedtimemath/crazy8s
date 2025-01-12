using MediatR;

namespace C8S.AdminApp.Client.Services.Pages;

public record GoToPageCommand(
    string PageName,
    int? IdValue = null): IRequest;