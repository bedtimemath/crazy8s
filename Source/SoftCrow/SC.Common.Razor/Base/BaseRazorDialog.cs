using Microsoft.AspNetCore.Components;
using Radzen;

namespace SC.Common.Razor.Base;

public abstract class BaseRazorDialog: BaseRazorComponent
{
    #region Injected Variables
    [Inject]
    protected DialogService DialogService { get; set; } = null!;
    #endregion
}