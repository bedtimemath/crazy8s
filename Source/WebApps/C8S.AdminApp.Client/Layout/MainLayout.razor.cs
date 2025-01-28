using System.Diagnostics;
using C8S.AdminApp.Client.Services.Navigation;
using C8S.AdminApp.Client.Services.Services;
using SC.Common.Client.Services;

namespace C8S.AdminApp.Client.Layout;

public partial class MainLayout(
    IServiceProvider serviceProvider)
{
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (!RendererInfo.IsInteractive) return;

        var pubSubService = serviceProvider.GetRequiredService<IPubSubService>() as AdminPubSubService ??
                            throw new UnreachableException(
                                "Injected IPubSubService must be of type AdminPubSubService");
        await pubSubService.InitializeAsync(serviceProvider);

        var navigationService = serviceProvider.GetRequiredService<INavigationService>();
        await navigationService.InitializeAsync(serviceProvider);
    }
}