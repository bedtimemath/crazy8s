using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.UI.Common;

public partial class RadzenPageContainer : ComponentBase
{
    #region Page Parameters
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter]
    public bool HideDuringPreRender { get; set; } = false;
    #endregion
}