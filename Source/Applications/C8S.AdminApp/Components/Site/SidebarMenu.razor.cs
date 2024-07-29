using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Site;

public partial class SidebarMenu: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Component LifeCycle

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    #endregion
}