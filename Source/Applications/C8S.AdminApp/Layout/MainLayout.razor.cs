using C8S.AdminApp.Services;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Layout;

public partial class MainLayout: LayoutComponentBase
{
    [Inject]
    public SelfService SelfService { get; set; } = default!;
}