using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace SC.Common.Razor.Components;

public partial class BusyCover : BaseRazorComponent
{
    #region Page Parameters
    [Parameter]
    public bool IsBusy { get; set; }
    #endregion
}