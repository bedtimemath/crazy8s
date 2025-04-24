using SC.Common.Responses;

namespace SC.Common.Helpers.CQRS.Interfaces;

public interface ICQRSListQuery<TResponse> : ICQRSQuery<WrappedListResponse<TResponse>>
    where TResponse: class
{
    public int? StartIndex { get; init; }
    public int? Count { get; init; }
    public string? Query { get; init; }
    public string? SortDescription { get; init; }
}