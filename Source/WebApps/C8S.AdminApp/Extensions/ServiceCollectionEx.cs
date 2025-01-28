using Microsoft.AspNetCore.ResponseCompression;

namespace C8S.AdminApp.Extensions;

public static class ServiceCollectionEx
{
    public static void AddSignalRServices(
        this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                ["application/octet-stream"]);
        });
    }
}
