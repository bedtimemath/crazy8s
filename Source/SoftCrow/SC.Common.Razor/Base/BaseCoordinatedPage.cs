using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Common.Helpers.Base;

namespace SC.Common.Razor.Base;

public abstract class BaseCoordinatedPage<TCoordinator> : OwningComponentBase<TCoordinator>
    where TCoordinator : class, ICoordinator
{
    #region Injected Variables
    [Inject]
    protected NotificationService NotificationService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;
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

    #region Protected Methods
    protected virtual async Task HandleExceptionRaisedAsync(Exception exception) =>
        await InvokeAsync(() => { HandleExceptionRaised(exception); });
    protected virtual void HandleExceptionRaised(Exception exception)
    {
        var exc = exception;
        while (exc != null)
        {
            NotificationService.Notify(NotificationSeverity.Error,
                exc.Message, exc.StackTrace);
            exc = exc.InnerException;
        }
    }
    #endregion
}