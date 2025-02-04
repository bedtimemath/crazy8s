using SC.Common.Interactions;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation.Queries;

public record CurrentUrlQuery : ICQRSQuery<DomainResponse<string>>
{
}