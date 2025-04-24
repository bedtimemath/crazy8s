using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace SC.Common.Razor.Navigation.Queries;

public record CurrentUrlQuery : ICQRSQuery<WrappedResponse<string>>
{
}