using C8S.Database.Abstractions.Base;
using C8S.Database.Abstractions.DTOs;

namespace C8S.AdminApp.Services;

public class HistoryService(
    ILogger<HistoryService> logger)
{
    #region Public Events
    public event EventHandler<HistoryEventArgs>? Changed;

    private void RaiseAdded(BaseDTO target) =>
        Changed?.Invoke(this, new HistoryEventArgs(HistoryAction.Add, target));
    private void RaiseRemoved(BaseDTO target) =>
        Changed?.Invoke(this, new HistoryEventArgs(HistoryAction.Remove, target));
    #endregion

    #region Private Variables
    public List<ApplicationDTO> Applications { get; } = new();
    #endregion

    #region Public Methods
    public void Add(ApplicationDTO applicationDTO)
    {
        if (Applications.Any(a => a.ApplicationId == applicationDTO.Id)) return;

        Applications.Add(applicationDTO);
        RaiseAdded(applicationDTO);
    }
    public void Remove(ApplicationDTO applicationDTO)
    {
        if (Applications.All(a => a.ApplicationId != applicationDTO.Id)) return;

        Applications.Remove(applicationDTO);
        RaiseRemoved(applicationDTO);
    }
    #endregion

}