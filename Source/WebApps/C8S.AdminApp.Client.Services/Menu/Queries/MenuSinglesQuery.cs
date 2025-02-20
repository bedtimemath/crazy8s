using C8S.AdminApp.Client.Services.Menu.Models;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Queries;

public record MenuSinglesQuery : ICQRSQuery<WrappedResponse<IEnumerable<MenuSingle>>>
{
    public bool ShowBeforeOthers { get; init; }
}