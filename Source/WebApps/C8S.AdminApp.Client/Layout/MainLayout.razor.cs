using C8S.AdminApp.Client.Services.Navigation;
using SC.Common.Client.Services;
using SC.Common.Interfaces;

namespace C8S.AdminApp.Client.Layout;

public partial class MainLayout(
    IServiceProvider serviceProvider)
{
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!RendererInfo.IsInteractive) return;

        var pubSubService = serviceProvider.GetRequiredService<IPubSubService>();
        if (pubSubService is IAsyncInitializable initializablePubSub)
            await initializablePubSub.InitializeAsync(serviceProvider);

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        if (navigationService is IAsyncInitializable initializableNavigation)
            await initializableNavigation.InitializeAsync(serviceProvider);
    }
}