using Microsoft.Extensions.DependencyInjection;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Helpers.Notifier.Services;
using SC.Common.Helpers.PassThrus;
using SC.Common.Helpers.PubSub.Services;
using SC.Common.Interfaces;

namespace SC.Common.Helpers.Extensions;

public static class ServiceCollectionEx
{
    public static IServiceCollection AddCommonHelpers(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeHelper, PassthruDateTimeHelper>();
        services.AddTransient<IRandomizer, PassthruRandomizer>();
        
        services.AddSingleton<ICQRSService, CQRSService>();
        services.AddSingleton<IPubSubService, PubSubService>();
        services.AddSingleton<INotifierService, NotifierService>();

        return services;
    }
}