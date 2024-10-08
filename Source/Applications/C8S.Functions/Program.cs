using Azure.Identity;
using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Common.Models;
using C8S.Database.Abstractions.Models;
using C8S.Database.Repository.Extensions;
using C8S.FullSlate.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.SystemConsole.Themes;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
Log.Logger.Information("Environment: {Environment}", environmentName);

try
{
    var host = new HostBuilder();

    host.ConfigureHostConfiguration(builder =>
    {
        Log.Logger.Information("Configuring Host Configuration");

        builder.AddJsonFile("local.settings.json", true)
            .AddEnvironmentVariables();

        var sensitiveFolderPath = Environment.GetEnvironmentVariable("C8S_SensitiveFolder");
        if (!String.IsNullOrEmpty(sensitiveFolderPath))
            builder.SetBasePath(sensitiveFolderPath)
                .AddJsonFile($"c8s.appsettings.{environmentName?.ToLower()}.json");
    });

    host.ConfigureAppConfiguration((hostContext, builder) =>
    {
        Log.Logger.Information("Configuring App Configuration");
                    
        // check for the two variables we need immediately
        var sensitiveFolderPath = Environment.GetEnvironmentVariable("C8S_SensitiveFolder");
        if (!String.IsNullOrEmpty(sensitiveFolderPath)) return;

        // configure with the azure configuration
        var appConfigCnnString = Environment.GetEnvironmentVariable("C8S_AppConfig");
        builder.AddAzureAppConfiguration(config =>
        {
            Log.Logger.Information("Connecting to Azure App Configuration using: {CnnString}", appConfigCnnString);
            config.Connect(appConfigCnnString)
                .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                .Select(KeyFilter.Any, LabelFilter.Null)
                .Select(KeyFilter.Any, environmentName);
        });
    });

    host.ConfigureFunctionsWebApplication();

    host.ConfigureServices((hostContext, services) =>
    {
        Log.Logger.Information("Configuring Services");

        var connections = hostContext.Configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                          throw new Exception($"Missing configuration section: {Connections.SectionName}");
        var apiKeys = hostContext.Configuration.GetSection(ApiKeys.SectionName).Get<ApiKeys>() ??
                      throw new Exception($"Missing configuration section: {ApiKeys.SectionName}");
        var endpoints = hostContext.Configuration.GetSection(Endpoints.SectionName).Get<Endpoints>() ??
                        throw new Exception($"Missing configuration section: {Endpoints.SectionName}");

        /*****************************************
         * AZURE CLIENTS SETUP
         */
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddQueueServiceClient(connections.AzureStorage);
            clientBuilder.AddBlobServiceClient(connections.AzureStorage);
        });

        /*****************************************
         * REPOSITORY SETUP
         */
        services.AddC8SRepository(connections.Database);

        /*****************************************
         * OTHER CRAZY 8s SETUP
         */
        if (String.IsNullOrEmpty(endpoints.FullSlateApi)) throw new Exception("Missing Endpoints:FullSlateApi");
        if (String.IsNullOrEmpty(apiKeys.FullSlate)) throw new Exception("Missing ApiKeys:FullSlate");
        services.AddFullSlateServices(endpoints.FullSlateApi, apiKeys.FullSlate);
        services.AddCommonHelpers();

        /*****************************************
         * TELEMETRY
         */
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    });

    host.UseSerilog((context, services, config) => config
        .MinimumLevel.Is(LogEventLevel.Verbose)
        .MinimumLevel.Override("EntityFrameworkCore.Triggered", LogEventLevel.Warning)
        .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: SharedConstants.Templates.DefaultConsoleLog,
            theme: AnsiConsoleTheme.Code)
        .WriteTo.ApplicationInsights(services.GetRequiredService<IConfiguration>()
            .GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"), new TraceTelemetryConverter())
    );

    SelfLog.Enable(m => Console.Error.WriteLine(m));

    host.Build().Run();
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
