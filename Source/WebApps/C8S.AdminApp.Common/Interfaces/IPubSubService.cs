using MediatR;

namespace C8S.AdminApp.Common.Interfaces;

public interface IPubSubService: IAsyncDisposable, IDisposable
{
    ValueTask InitializeAsync();
    void Subscribe<T>(Func<T, Task> handler) where T : class;
    void Unsubscribe<T>(Func<T, Task> handler) where T : class;
    Task Publish<T>(T notification) where T : class;
}