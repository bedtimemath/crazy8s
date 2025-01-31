using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Menu;

public sealed partial class SidebarGroup: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<SidebarGroup> Logger { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public string Display { get; set; } = null!;
    
    [Parameter]
    public string IconString { get; set; } = null!;

    [Parameter]
    public EventCallback Clicked { get; set; }
    #endregion

    #region Private Variables
    private bool _isSelected = false;
    #endregion

    #region Public Methods
    public async Task SetSelected(bool isSelected)
    {
        if (_isSelected == isSelected)  return;

        _isSelected = isSelected;
        await InvokeAsync(StateHasChanged);
    }
    #endregion
}