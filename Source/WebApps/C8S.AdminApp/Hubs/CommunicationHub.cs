using C8S.AdminApp.Common;
using Microsoft.AspNetCore.SignalR;
using SC.Audit.Abstractions.Models;

namespace C8S.AdminApp.Hubs;

public class CommunicationHub: Hub
{
    public async Task RaiseDataChange(DataChange dataChange) => 
        await Clients.All.SendAsync(AdminAppConstants.Messages.DataChange, dataChange);
}