using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.CQRS.Interfaces;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.PubSub.Services;

namespace SC.Common.Helpers.Base;

public abstract class BaseCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : ICoordinator
{
    #region Public Events
    public event EventHandler? IsBusyChanged;
    public void RaiseIsBusyChanged() => IsBusyChanged?.Invoke(this, EventArgs.Empty);
    #endregion

    #region Public Properties (IsBusy)
    public bool IsBusy
    {
        get => _isBusy;
        protected set
        {
            if (value == _isBusy) return;

            _isBusy = value; 
            RaiseIsBusyChanged();
        }
    }
    private bool _isBusy;
    #endregion

    #region Private Variables
    private CancellationTokenSource? _cancellationTokenSource = null;
    #endregion

    #region ReadOnly Public Properties
    public ILoggerFactory LoggerFactory => loggerFactory;
    public IPubSubService PubSubService => pubSubService;
    #endregion

    #region Public Properties
    public Func<Task>? ComponentRefresh { get; set; }
    #endregion

    #region Constructors / Destructors
    ~BaseCoordinator()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        TearDownUnmanaged();
        if (disposing)
        {
            _cancellationTokenSource?.Dispose();
            TearDown();
        }
    }
    #endregion

    #region Overridable Methods
    public virtual void SetUp() { }
    public virtual void TearDown() { }
    public virtual void TearDownUnmanaged() { }
    #endregion

    #region Protected Methods
    protected async Task PerformComponentRefresh()
    {
        if (ComponentRefresh == null) return;
        await ComponentRefresh.Invoke().ConfigureAwait(false);
    }

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
    protected async Task ExecuteCommand<TCommand>(TCommand command)
        where TCommand : class, ICQRSCommand
    {
        using (_cancellationTokenSource = new CancellationTokenSource())
        {
            await cqrsService.ExecuteCommand<TCommand>(command, _cancellationTokenSource.Token);
        }
        _cancellationTokenSource = null;
    }
    protected async Task<TResponse> GetCommandResults<TCommand, TResponse>(TCommand command)
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