using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Radzen;

namespace C8S.Common.Razor.Base;

public abstract class BaseRazorDialog: BaseRazorComponent
{
    #region Injected Variables
    [Inject]
    protected ILogger<BaseRazorDialog> BaseLogger { get; set; } = default!;
    [Inject]
    protected DialogService DialogService { get; set; } = default!;
    [Inject]
    protected NotificationService NotificationService { get; set; } = default!;
    #endregion

    #region Exception Methods
    // since the calling page doesn't watch for raised exceptions, we need
    //  to handle the notifications ourselves, rather than as the base component would
    protected override async Task RaiseExceptionAsync(Exception exception) =>
        await InvokeAsync(() => { RaiseException(exception); });
    protected override void RaiseException(Exception exception)
    {
        var exc = exception;
        while (exc != null)
        {
            BaseLogger.LogError(exc, exc.Message);
            NotificationService.Notify(NotificationSeverity.Error,
                exc.Message, exc.StackTrace);
            exc = exc.InnerException;
        }
    }
    #endregion
}