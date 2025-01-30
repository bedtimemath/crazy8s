using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Common.PubSub;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Navigation;

public sealed class NavigationService(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    NavigationManager navigationManager) : INavigationService
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<NavigationService> _logger = loggerFactory.CreateLogger<NavigationService>();
    public readonly Guid UniqueIdentifier = Guid.NewGuid();
    #endregion

    public void Initialize(IServiceProvider provider)
    {
        //_logger.LogDebug("[{Guid:D}] Initializing", UniqueIdentifier);
        //_pubSubService = provider.GetRequiredService<IPubSubService>();
        //_logger.LogDebug("[{Guid:D}] Loading PubSubService: {PSGuid:D}", UniqueIdentifier, _pubSubService.UniqueIdentifier );

        //_navigationManager.RegisterLocationChangingHandler(HandleLocationChanging);
    }

    public void Dispose()
    {
    }

    #region Command Handlers
    public async Task Handle(OpenPageCommand openPageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Guid:D}] Open: {PageName} [{IdValue}]", UniqueIdentifier, openPageCommand.PageUrl, openPageCommand.IdValue);

        var newUrl = openPageCommand.PageUrl;
        if (openPageCommand.IdValue != null) newUrl += $"/{openPageCommand.IdValue}";
        var currentUrl = GetCurrentUrl();

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = newUrl,
            Action = PageChangeAction.Open,
            IdValue = openPageCommand.IdValue
        }); 

        await Task.Run(() => navigationManager.NavigateTo(newUrl), cancellationToken);
    }

    public async Task Handle(ClosePageCommand closePageCommand, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{Guid:D}] Close: {PageName} [{IdValue}]", UniqueIdentifier, closePageCommand.PageUrl, closePageCommand.IdValue);

        var currentUrl = GetCurrentUrl();
        if (currentUrl != closePageCommand.PageUrl) return;

        // todo: drop back correctly
        var newUrl = AdminAppConstants.Pages.RequestsList;

        await pubSubService.Publish(new PageChange()
        {
            CurrentUrl = currentUrl,
            NewUrl = newUrl,
            Action = PageChangeAction.Close,
            IdValue = closePageCommand.IdValue
        });

        await Task.Run(() => navigationManager.NavigateTo(newUrl), cancellationToken);
    }
    #endregion

    #region Private Methods
    private string GetCurrentUrl() => navigationManager.ToBaseRelativePath(navigationManager.Uri);
    #endregion

    public ValueTask HandleLocationChanging(LocationChangingContext context)
    {
        _logger.LogDebug("Context={@Context}", context);
        return ValueTask.CompletedTask;
    }
}