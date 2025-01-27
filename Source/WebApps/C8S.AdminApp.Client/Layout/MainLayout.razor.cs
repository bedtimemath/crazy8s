using SC.Common.Client.Services;

namespace C8S.AdminApp.Client.Layout;

public partial class MainLayout(
    IServiceProvider serviceProvider)
{
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (RendererInfo.IsInteractive)
        {
            var communicationService = serviceProvider.GetRequiredService<PubSubService>();
            await communicationService.InitializeAsync();
        }
    }
}