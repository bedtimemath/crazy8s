using Microsoft.AspNetCore.Components;
using SC.Common.Helpers.Base;

namespace SC.Common.Razor.Base;

public abstract class BaseCoordinatedComponent<TCoordinator> : OwningComponentBase<TCoordinator>
    where TCoordinator : class, ICoordinator
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

    #region Component Parameters (Callbacks)
    [Parameter]
    public EventCallback<Exception> ExceptionRaised { get; set; }
    #endregion

    #region Protected Properties
    protected override bool ShouldRender() => _shouldRender;
    private bool _shouldRender = true;

    protected void PauseRendering() => _shouldRender = false;
    protected void ResumeRendering() => _shouldRender = true;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Service.SetUp();
        Service.ComponentRefresh = async () => await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        Service.TearDown();
        Service.ComponentRefresh = null;
    }
    #endregion
}