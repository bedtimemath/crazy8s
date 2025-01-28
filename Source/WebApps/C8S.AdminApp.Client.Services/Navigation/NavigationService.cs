using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Common.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public sealed class NavigationService(
    ILoggerFactory loggerFactory) : INavigationService //, IRequestHandler<OpenNavigation>
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NavigationService> _logger = loggerFactory.CreateLogger<NavigationService>();
    #endregion

    public ValueTask InitializeAsync(IServiceProvider provider)
    {
        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
    }

    //public Task Handle(OpenNavigation request, CancellationToke
    //n cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}

    public ValueTask HandleLocationChanging(LocationChangingContext context)
    {
        _logger.LogDebug("Context={@Context}", context);
        return ValueTask.CompletedTask;
    }
}