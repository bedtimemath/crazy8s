using SC.Common.Client.Services;

namespace C8S.AdminApp.Client.Layout;

public partial class MainLayout(
    ILoggerFactory loggerFactory,
    IServiceProvider serviceProvider)
{
    private readonly ILogger<MainLayout> _logger = loggerFactory.CreateLogger<MainLayout>();

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