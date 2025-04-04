using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Abstractions.Models;

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
    private void HandleNotificationReceived(object? sender, NotificationEventArgs args)
    {
        var severity = args.Notification.Level switch
        {
            NotificationLevel.Error => NotificationSeverity.Error,
            NotificationLevel.Info => NotificationSeverity.Info,
            NotificationLevel.Success => NotificationSeverity.Success,
            NotificationLevel.Warning => NotificationSeverity.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(args.Notification.Level))
        };
        var summary = args.Notification.Summary ?? args.Notification.Level switch
        {
            NotificationLevel.Error => "Error Message",
            NotificationLevel.Info => "Information",
            NotificationLevel.Success => "Success",
            NotificationLevel.Warning => "Warning",
            _ => throw new ArgumentOutOfRangeException(nameof(args.Notification.Level))
        };

        var notificationMessage = new NotificationMessage()
        {
            Style = "position: absolute;",
            Severity = severity,
            Summary = summary,
            Detail = args.Notification.Detail,
            CloseOnClick = true
        };
        NotificationService.Notify(notificationMessage);
    } 
    #endregion
}