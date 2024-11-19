using MediatR;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Notifications;

public class DataChangeNotification(
    DataChange dataChange): INotification
{
    public DataChange DataChange { get; private set; } = dataChange;
}