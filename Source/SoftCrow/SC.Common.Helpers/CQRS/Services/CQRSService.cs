using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.CQRS.Interfaces;

namespace SC.Common.Helpers.CQRS.Services;

public sealed class CQRSService(
    ILoggerFactory loggerFactory) : ICQRSService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<CQRSService> _logger = loggerFactory.CreateLogger<CQRSService>();
    #endregion

    #region Private Variables
    private readonly Dictionary<string, dynamic> _handlers = [];
    #endregion

    #region Registration Methods
    public void RegisterCommand<TCommand>(Func<TCommand, CancellationToken, Task> function)
        where TCommand : class, ICQRSCommand
    {
        if (!_handlers.TryAdd(typeof(TCommand).Name, function))
            throw new UnreachableException($"Already registered handler for '{typeof(TCommand).Name}'");
    }

    public void RegisterCommand<TCommand, TResponse>(Func<TCommand, CancellationToken, Task<TResponse>> function)
        where TCommand : class, ICQRSCommand<TResponse>
        where TResponse : class
    {
        if (!_handlers.TryAdd(typeof(TCommand).Name, function))
            throw new UnreachableException($"Already registered handler for '{typeof(TCommand).Name}'");
    }

    public void RegisterQuery<TQuery, TResponse>(Func<TQuery, CancellationToken, Task<TResponse>> function)
        where TQuery : class, ICQRSQuery<TResponse>
        where TResponse : class
    {
        if (!_handlers.TryAdd(typeof(TQuery).Name, function))
            throw new UnreachableException($"Already registered handler for '{typeof(TQuery).Name}'");
    }
    #endregion

    #region ExecutionMethods
    public async Task ExecuteCommand<TCommand>(TCommand command, CancellationToken cancellationToken = default)
    where TCommand : class, ICQRSCommand
    {
        //_logger.LogInformation("ExecuteCommand<{CommandType}>({@Command})", typeof(TCommand).Name, command);

        if (!_handlers.TryGetValue(typeof(TCommand).Name, out var dynamicValue))
            throw new UnreachableException($"Could not find handler for type: {typeof(TCommand).Name}");

        var handler = dynamicValue as Func<TCommand, CancellationToken, Task> ??
                      throw new UnreachableException($"Could not cast function as command handler for {typeof(TCommand).Name}");
        await handler(command, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> ExecuteResponseCommand<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICQRSCommand<TResponse>
        where TResponse : class
    {
        //_logger.LogInformation("ExecuteCommand<{CommandType}>({@Command})", typeof(TCommand).Name, command);

        if (!_handlers.TryGetValue(typeof(TCommand).Name, out var dynamicValue))
            throw new UnreachableException($"Could not find handler for type: {typeof(TCommand).Name}");

        var handler = dynamicValue as Func<TCommand, CancellationToken, Task<TResponse>> ??
                      throw new UnreachableException($"Could not cast function as command handler for {typeof(TCommand).Name}");
        return await handler(command, cancellationToken);
    }

    public async Task<TResponse> ExecuteQuery<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class, ICQRSQuery<TResponse>
        where TResponse : class, new()
    {
        //_logger.LogInformation("ExecuteQuery<{QueryType}>({@Query})", typeof(TQuery).Name, query);

        if (!_handlers.TryGetValue(typeof(TQuery).Name, out var dynamicValue))
            throw new UnreachableException($"Could not find handler for type: {typeof(TQuery).Name}");

        var handler = dynamicValue as Func<TQuery, CancellationToken, Task<TResponse>> ??
                      throw new UnreachableException($"Could not cast function as command handler for {typeof(TQuery).Name}");
        return await handler(query, cancellationToken);
    } 
    #endregion
}