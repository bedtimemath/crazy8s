namespace SC.Common.Interfaces;

public interface IAsyncInitializable
{
    ValueTask InitializeAsync(IServiceProvider provider);
}