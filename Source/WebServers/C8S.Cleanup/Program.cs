using Azure.Identity;
using C8S.DrawDown.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SC.Common;
using SC.SendGrid.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    // NOTE: wanted to use the following, but couldn't see how serilog is integrated in this version
    // var builder = FunctionsApplication.CreateBuilder(args);
    var builder = new HostBuilder()
        .ConfigureFunctionsWebApplication()
        .ConfigureAppConfiguration((context, builder) =>
        {
            var environsConfig = builder.AddEnvironmentVariables().Build();

            var environmentName = environsConfig["ASPNETCORE_ENVIRONMENT"];
            var appConfigCnnString = environsConfig["C8S_AppConfig"];
            var configFolderPath = environsConfig["C8S_ConfigFolder"];

            if (String.IsNullOrEmpty(configFolderPath))
            {
                // configure with the azure configuration
                builder.AddAzureAppConfiguration(config =>
                {
                    config.Connect(appConfigCnnString)
                        .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                        .Select(KeyFilter.Any, LabelFilter.Null)
                        .Select(KeyFilter.Any, environmentName);
                });
            }
            else
            {
                // configure with a file (much faster)
                builder
                    .SetBasePath(configFolderPath)
                    .AddJsonFile($"c8s.appsettings.{environmentName}.json", optional: false);
            }
        })
        .ConfigureServices((context, services) =>
        {
            /*****************************************
             * COMMUNICATION SERVICES
             */
            services.AddSendGridServices();

            /*****************************************
             * LOCAL SERVICES
             */
            services.AddDrawDownServices();
        })
        .ConfigureLogging((context, builder) =>
        {
            var environmentName = context.Configuration["ASPNETCORE_ENVIRONMENT"];

            var levelSwitch = new LoggingLevelSwitch(environmentName == SoftCrowConstants.Platforms.Development ?
                LogEventLevel.Verbose : LogEventLevel.Warning);
            builder.Services.AddSingleton(levelSwitch);

            builder.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .CreateLogger());
            SelfLog.Enable(m => Console.Error.WriteLine(m));

        });
    
    builder.Build().Run();
}
catch (Exception ex)
{
    // see: https://githubmate.com/repo/dotnet/runtime/issues/60600
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) { throw; }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Logger.Information("Shutdown Complete");
    Log.CloseAndFlush();
}
