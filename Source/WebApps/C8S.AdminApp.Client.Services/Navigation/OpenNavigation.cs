using MediatR;

namespace C8S.AdminApp.Client.Services.Navigation;

public record OpenNavigation: IRequest
{
    public NavigationGroup Group { get; init; }
    public int? IdValue { get; init; }
}