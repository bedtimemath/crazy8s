using Azure.Identity;
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
#if false // !DEBUG
using Microsoft.Azure.Functions.Worker;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;
using TGPL.Common;
using TGPL.Common.Configs;
#endif

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
                    .AddJsonFile($"tgpl.appsettings.{environmentName}.json", optional: false);
            }
        })
        .ConfigureServices((context, services) =>
        {
#if false // !DEBUG
            /*****************************************
             * APPLICATION INSIGHTS
             */
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
#endif

            /*****************************************
             * COMMUNICATION SERVICES
             */
            services.AddSendGridServices();

        })
        .ConfigureLogging((context, builder) =>
        {
            var environmentName = context.Configuration["ASPNETCORE_ENVIRONMENT"];

            var levelSwitch = new LoggingLevelSwitch(environmentName == SoftCrowConstants.Platforms.Development ?
                LogEventLevel.Verbose : LogEventLevel.Warning);
            builder.Services.AddSingleton(levelSwitch);
            
#if false // !DEBUG
            var connections = context.Configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                              throw new Exception($"Missing configuration section: {Connections.SectionName}");
#endif    

            builder.AddSerilog(new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
#if false // !DEBUG
                .WriteTo.MSSqlServer(
                    connectionString: connections.AuditDatabase,
                    sinkOptions: new MSSqlServerSinkOptions()
                    {
                        AutoCreateSqlTable = true, 
                        TableName = TGPLConstants.LogTables.FunctionsLog, 
                        LevelSwitch = levelSwitch
                    })
                .WriteTo.ApplicationInsights(context.Configuration
                        .GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"), 
                    telemetryConverter: new TraceTelemetryConverter())
                .WriteTo.Console(
                    outputTemplate: SoftCrowConstants.Templates.DefaultConsoleLog,
                    theme: AnsiConsoleTheme.Code)
#endif    
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
