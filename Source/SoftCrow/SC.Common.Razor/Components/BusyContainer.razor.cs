using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace SC.Common.Razor.Components;

public partial class BusyContainer : BaseRazorComponent
{
    #region Page Parameters
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter]
    public bool IsBusy { get; set; }
    #endregion
}