using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.Domain.Features;
using SC.Common.Responses;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Queries;

public record MenuItemsQuery : ICQRSQuery<WrappedResponse<IEnumerable<MenuItem>>>
{
    public DomainEntity Entity { get; init; }
}