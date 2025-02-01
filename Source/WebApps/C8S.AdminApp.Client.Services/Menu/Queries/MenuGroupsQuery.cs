using C8S.AdminApp.Client.Services.Menu.Models;
using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Menu.Queries;

public record MenuGroupsQuery : ICQRSQuery<DomainResponse<IEnumerable<MenuGroup>>>
{
}