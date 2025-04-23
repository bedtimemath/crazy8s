using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.Notifier.Enums;
using SC.Common.Helpers.Notifier.Services;

namespace C8S.AdminApp.Client.Layout;

public partial class MainLayout : LayoutComponentBase, IDisposable
{
    #region Injected Properties
    [Inject]
    public ICQRSService CQRSService { get; set; } = null!;
    
    [Inject]
    public INotifierService NotifierService { get; set; } = null!;

    [Inject]
    public NotificationService NotificationService { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        base.OnInitialized();
        NotifierService.NotificationReceived += HandleNotificationReceived;
    }

    public void Dispose()
    {
        NotifierService.NotificationReceived -= HandleNotificationReceived;
    }

#if false // not necessary
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;

        // let the navigation service know that the first page has been loaded
        await CQRSService
            .ExecuteCommand(new NavigationCommand()
            {
                Action = NavigationAction.Initialize,
                PageUrl = NavigationManager.GetRelativeUrl(),
            })
            .ConfigureAwait(false);
    } 
#endif
    #endregion

    #region Event Handlers
    private void HandleNotificationReceived(object? sender, NotifierEventArgs args)
    {
        var severity = args.NotifierMessage.Level switch
        {
            NotifierSeverity.Error => NotificationSeverity.Error,
            NotifierSeverity.Info => NotificationSeverity.Info,
            NotifierSeverity.Success => NotificationSeverity.Success,
            NotifierSeverity.Warning => NotificationSeverity.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(args.NotifierMessage.Level))
        };
        var summary = args.NotifierMessage.Summary ?? args.NotifierMessage.Level switch
        {
            NotifierSeverity.Error => "Error Message",
            NotifierSeverity.Info => "Information",
            NotifierSeverity.Success => "Success",
            NotifierSeverity.Warning => "Warning",
            _ => throw new ArgumentOutOfRangeException(nameof(args.NotifierMessage.Level))
        };

        var notificationMessage = new NotificationMessage()
        {
            Style = "position: absolute;",
            Severity = severity,
            Summary = summary,
            Detail = args.NotifierMessage.Detail,
            CloseOnClick = true
        };
        NotificationService.Notify(notificationMessage);
    } 
    #endregion
}