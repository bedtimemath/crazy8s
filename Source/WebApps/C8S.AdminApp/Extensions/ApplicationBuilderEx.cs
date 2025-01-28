using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Hubs;

namespace C8S.AdminApp.Extensions;

public static class ApplicationBuilderEx
{
    public static IApplicationBuilder UseSignalRServices(this WebApplication builder)
    {
        builder.UseResponseCompression();
        builder.MapHub<CommunicationHub>("/" + AdminAppConstants.HubEndpoints.Communication);

        return builder;
    }
}