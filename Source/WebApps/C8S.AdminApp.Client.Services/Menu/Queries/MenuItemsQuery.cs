using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.Domain.Features;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace C8S.AdminApp.Client.Services.Menu.Queries;

public record MenuItemsQuery : ICQRSQuery<WrappedResponse<IEnumerable<MenuItem>>>
{
    public DomainEntity Entity { get; init; }
}