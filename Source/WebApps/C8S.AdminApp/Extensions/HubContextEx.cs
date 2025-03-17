using System.Text.Json;
using C8S.AdminApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using SC.Common;
using SC.Common.PubSub;

namespace C8S.AdminApp.Extensions;

public static class HubContextEx
{
    public static async Task SendDataChange(this IHubContext<CommunicationHub> hub,
        DataChangeAction action, string entityName) =>
        await hub.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange,
            new DataChange() { Action = action, EntityName = entityName });

    public static async Task SendDataChange<TModel>(this IHubContext<CommunicationHub> hub,
        DataChangeAction action, string entityName, int entityId, TModel details) 
    where TModel : class =>
        await SendDataChange(hub, action, entityName, entityId, 
            JsonSerializer.Serialize(details));

    public static async Task SendDataChange(this IHubContext<CommunicationHub> hub,
        DataChangeAction action, string entityName, int entityId, string jsonDetails) =>
        await hub.Clients.All.SendAsync(SoftCrowConstants.Messages.DataChange,
            new DataChange()
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                JsonDetails = jsonDetails
            });
}