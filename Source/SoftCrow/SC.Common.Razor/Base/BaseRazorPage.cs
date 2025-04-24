using Microsoft.AspNetCore.Components;
using Radzen;

namespace SC.Common.Razor.Base;

public abstract class BaseRazorPage: ComponentBase
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