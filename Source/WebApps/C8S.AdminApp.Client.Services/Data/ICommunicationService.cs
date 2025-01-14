using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Client.Services.Data;

public interface ICommunicationService
{
    event EventHandler<DataChangedEventArgs>? DataChanged;
    Task InitializeAsync();
}