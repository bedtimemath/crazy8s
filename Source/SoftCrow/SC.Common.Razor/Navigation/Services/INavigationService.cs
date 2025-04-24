using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Interfaces;
using SC.Common.Razor.Navigation.Commands;
using SC.Common.Razor.Navigation.Queries;
using SC.Common.Responses;

namespace SC.Common.Razor.Navigation.Services;

public interface INavigationService :  IServiceInitializable, IDisposable,
    ICQRSCommandHandler<NavigationCommand>,
    ICQRSQueryHandler<CurrentUrlQuery, WrappedResponse<string>>
{
}