using C8S.AdminApp.Client.Services.Pages;
using MediatR;
using SC.Common.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public interface INavigationService: IInitializable, IDisposable,
    IRequestHandler<OpenPageCommand>,
    IRequestHandler<ClosePageCommand>
{
}