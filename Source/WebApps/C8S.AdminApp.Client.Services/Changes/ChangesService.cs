using MediatR;
using Microsoft.Extensions.Logging;
using SC.Audit.Abstractions.Notifications;

namespace C8S.AdminApp.Client.Services.Changes;

public class ChangesService(
    ILoggerFactory loggerFactory): INotificationHandler<DataChangeNotification>
{
    private readonly ILogger<ChangesService> _logger = loggerFactory.CreateLogger<ChangesService>();

    public Task Handle(DataChangeNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Notification: {@Notification}", notification);
        return Task.CompletedTask;
    }
}