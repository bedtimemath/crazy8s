namespace C8S.AdminApp.Services;

#if false
public class ChangesService(
ILoggerFactory loggerFactory) : INotificationHandler<DataChangeNotification>
{
    private readonly ILogger<ChangesService> _logger = loggerFactory.CreateLogger<ChangesService>();

    public Task Handle(DataChangeNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Notification: {@Notification}", notification);
        return Task.CompletedTask;
    }
} 
#endif