using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Responses;

namespace SC.Common.Helpers.CQRS.Base;

public abstract record BaseListQuery<TResponse> :  IListQuery,
    ICQRSQuery<WrappedListResponse<TResponse>>
    where TResponse: class
{
    public int? StartIndex { get; init; }
    public int? Count { get; init; }
    public string? Query { get; init; }
    public string? SortDescription { get; init; }
}