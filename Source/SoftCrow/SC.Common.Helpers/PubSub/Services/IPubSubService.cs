namespace SC.Common.Helpers.PubSub.Services;

public interface IPubSubService
{
    void Subscribe<T>(Func<T, Task> handler) where T : class;
    void Unsubscribe<T>(Func<T, Task> handler) where T : class;
    void Publish<T>(T notification) where T : class;
    void PublishException(Exception exception);
}