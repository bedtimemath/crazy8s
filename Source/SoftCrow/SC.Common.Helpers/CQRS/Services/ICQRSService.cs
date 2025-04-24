using SC.Common.Helpers.CQRS.Interfaces;

namespace SC.Common.Helpers.CQRS.Services;

public interface ICQRSService
{
    void RegisterCommand<TCommand>(Func<TCommand, CancellationToken, Task> function)
        where TCommand : class, ICQRSCommand;

    void RegisterCommand<TCommand, TResponse>(Func<TCommand, CancellationToken, Task<TResponse>> function)
        where TCommand : class, ICQRSCommand<TResponse>
        where TResponse : class;

    void RegisterQuery<TQuery, TResponse>(Func<TQuery, CancellationToken, Task<TResponse>> function)
        where TQuery : class, ICQRSQuery<TResponse>
        where TResponse : class;

    public Task ExecuteCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICQRSCommand;

    public Task<TResponse> ExecuteResponseCommand<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICQRSCommand<TResponse>
        where TResponse : class;

    Task<TResponse> ExecuteQuery<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class, ICQRSQuery<TResponse>
        where TResponse : class, new();
}