namespace SC.Common.Helpers.CQRS.Interfaces;

public interface ICQRSQueryHandler<in TQuery, TResponse>
    where TQuery: class, ICQRSQuery<TResponse>
    where TResponse: class
{
    Task<TResponse> Handle(TQuery query, CancellationToken token = default);
}