using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Common.Interfaces;

public interface ICommunicationService
{
    event EventHandler<DataChangeEventArgs>? DataChanged;
    Task InitializeAsync();
}