using Microsoft.AspNetCore.Components;

namespace SC.Common.Razor.Base;

public abstract class BaseRazorComponent : ComponentBase
{
    #region Component Parameters (Standard)
    [Parameter]
    public string? ComponentId { get; set; } = null;

    [Parameter]
    public string? ComponentName { get; set; } = null;

    [Parameter]
    public string? CssClass { get; set; } = null;

    [Parameter]
    public bool IsDisabled { get; set; } = false;
    #endregion

    #region Protected Properties
    protected override bool ShouldRender() => _shouldRender;
    private bool _shouldRender = true;

    protected void PauseRendering() => _shouldRender = false;
    protected void ResumeRendering() => _shouldRender = true;
    #endregion
}