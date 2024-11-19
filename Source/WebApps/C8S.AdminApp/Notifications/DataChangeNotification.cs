using MediatR;
using SC.Audit.EFCore.Models;

namespace C8S.AdminApp.Notifications;

public class DataChangeNotification(
    DataChangeDb dataChange): INotification
{
    public DataChangeDb DataChange { get; private set; } = dataChange;
}