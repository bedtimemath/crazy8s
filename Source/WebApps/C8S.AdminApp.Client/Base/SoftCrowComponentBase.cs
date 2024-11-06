using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Base;

public abstract class SoftCrowComponentBase : ComponentBase
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

    #region Page Parameters (Callbacks)
    [Parameter]
    public EventCallback<Exception> ExceptionRaised { get; set; }
    #endregion

    #region Protected Properties
    protected override bool ShouldRender() => _shouldRender;
    private bool _shouldRender = true;

    protected void PauseRendering() => _shouldRender = false;
    protected void ResumeRendering() => _shouldRender = true;
    #endregion

    #region Page LifeCycle
    protected bool HasRendered = false;
    protected Exception? PreException = null;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender && PreException != null)
        {
            await ExceptionRaised.InvokeAsync(PreException);
            PreException = null;
        }

        HasRendered = true;
    }
    #endregion

    #region Exception Methods
    // we can't raise the exception if we've not yet rendered
    protected virtual async Task RaiseExceptionAsync(Exception exception)
    {
        if (HasRendered)
            await ExceptionRaised.InvokeAsync(exception);
        else
            PreException = exception;
    }

    // is this the best way to change to synchronous?
    protected virtual  void RaiseException(Exception exception) => _ = RaiseExceptionAsync(exception);
    #endregion
}