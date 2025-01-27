using C8S.AdminApp.Client.Services.Pages;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using SC.Common.Client.Services;
using SC.Common.PubSub;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarGroup: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarGroup> Logger { get; set; } = null!;

    [Inject]
    public PagesService PagesService { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public PageGroup Group { get; set; } = null!;

    [Parameter]
    public EventCallback<PageGroup> Clicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected;
    #endregion

    #region Component LifeCycle

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Logger.LogDebug("{Mine}={Current}", Group.Url, PagesService.CurrentUrl);
    }

    #endregion
}