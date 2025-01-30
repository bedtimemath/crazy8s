using SC.Messaging.Abstractions.Interfaces;
using System.Diagnostics;

namespace C8S.AdminApp.Client.Services.Coordinators;

public abstract class BaseCoordinator(
    ICQRSService cqrsService)
{
    #region Private Variables
    private CancellationTokenSource? _cancellationTokenSource = null;
    #endregion

    #region Constructors Destructors
    ~BaseCoordinator()
    {
        if (_cancellationTokenSource == null) return;

        try { _cancellationTokenSource.Cancel(); }
        catch
        {
            // we don't want this to break us
        }
    }
    #endregion

    #region Protected Methods
    protected async Task<TResult> GetQueryResults<TQuery, TResult>(TQuery query)
        where TQuery : class, ICQRSQuery<TResult>
        where TResult : class, new()
    {
        TResult result;
        using (_cancellationTokenSource = new CancellationTokenSource())
        {
            result = (await cqrsService.ExecuteQuery<TQuery, TResult>(query, _cancellationTokenSource.Token)) ??
                     throw new UnreachableException("Could not convert to TResult");
        }
        _cancellationTokenSource = null;
        return result!;
    }
    protected  async Task ExecuteCommand<TCommand>(TCommand command)
        where TCommand : class, ICQRSCommand
    {
        using (_cancellationTokenSource = new CancellationTokenSource())
        {
            await cqrsService.ExecuteCommand<TCommand>(command, _cancellationTokenSource.Token);
        }
        _cancellationTokenSource = null;
    }
    protected  async Task<TResponse> GetCommandResults<TCommand, TResponse>(TCommand command)
        where TCommand : class, ICQRSCommand<TResponse>
        where TResponse : class, new()
    {
        TResponse response;
        using (_cancellationTokenSource = new CancellationTokenSource())
        {
            response = (await cqrsService.ExecuteResponseCommand<TCommand, TResponse>(command, _cancellationTokenSource.Token)) ??
                     throw new UnreachableException("Could not convert to TResult");
        }
        _cancellationTokenSource = null;
        return response!;
    }
    #endregion
}