using C8S.AdminApp.Client.Services.Menu.Models;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Menu.Queries;

public record MenuSinglesQuery : ICQRSQuery<WrappedResponse<IEnumerable<MenuSingle>>>
{
    public bool ShowBeforeOthers { get; init; }
}