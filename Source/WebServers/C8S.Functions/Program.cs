using Azure.Identity;
using C8S.Domain.AppConfigs;
using C8S.Domain.EFCore.Extensions;
using C8S.FullSlate.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SC.Audit.EFCore.Extensions;
using SC.Common;
using SC.Common.Helpers.Extensions;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.SystemConsole.Themes;
using System.Net.Http.Headers;
using C8S.Domain;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var host = new HostBuilder();

    var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
    Log.Logger.Information("Environment: {Environment}", environmentName);


    // check for the two variables we need immediately
    var appConfig = Environment.GetEnvironmentVariable("C8S_AppConfig");
    var configFolder = Environment.GetEnvironmentVariable("C8S_ConfigFolder");

    // create a level switch for logging
    var levelSwitch = new LoggingLevelSwitch(environmentName == SoftCrowConstants.Platforms.Development ? 
            LogEventLevel.Verbose : LogEventLevel.Warning);

    host.ConfigureHostConfiguration(builder =>
    {
        Log.Logger.Information("Configuring Host Configuration");

        builder.AddEnvironmentVariables();

        if (!String.IsNullOrEmpty(configFolder))
            builder.SetBasePath(configFolder)
                .AddJsonFile($"c8s.appsettings.{environmentName?.ToLower()}.json");
    });

    host.ConfigureAppConfiguration((hostContext, builder) =>
    {
        Log.Logger.Information("Configuring App Configuration");
                    
        // if we're using the config folder, the app config isn't needed
        if (!String.IsNullOrEmpty(configFolder)) return;

        // configure with the azure configuration
        builder.AddAzureAppConfiguration(config =>
        {
            Log.Logger.Information("Connecting to Azure App Configuration using: {CnnString}", appConfig);
            config.Connect(appConfig)
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
         * LOG LEVEL SWITCH
         */
        services.AddSingleton(levelSwitch);

        /*****************************************
         * AZURE CLIENTS SETUP
         */
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddQueueServiceClient(connections.AzureStorage);
            clientBuilder.AddBlobServiceClient(connections.AzureStorage);
        });

        /*****************************************
         * HTTP CLIENTS
         */
        if (String.IsNullOrEmpty(endpoints.C8SAdminApp)) throw new Exception("Missing Endpoints:C8SAdminApp");
        services.AddHttpClient(nameof(endpoints.C8SAdminApp), client =>
        {
            client.BaseAddress = new Uri(endpoints.C8SAdminApp);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        /*****************************************
         * SOFT CROW & LOCAL
         */
        services.AddCommonHelpers();
        services.AddSCAuditContext(connections.Audit);
        services.AddC8SDbContext(connections.Database);

        if (String.IsNullOrEmpty(endpoints.FullSlateApi)) throw new Exception("Missing Endpoints:FullSlateApi");
        if (String.IsNullOrEmpty(apiKeys.FullSlate)) throw new Exception("Missing ApiKeys:FullSlate");
        services.AddFullSlateServices(endpoints.FullSlateApi, apiKeys.FullSlate);

        /*****************************************
         * TELEMETRY
         */
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    });

    host.UseSerilog((context, services, config) =>
    {
        var auditCnnString = context.Configuration.GetSection(Connections.SectionName).Get<Connections>()?.Audit ??
                              throw new Exception($"Missing configuration section: {Connections.SectionName}");
        config
            .MinimumLevel.ControlledBy(levelSwitch)
            .MinimumLevel.Override("EntityFrameworkCore.Triggered", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: SoftCrowConstants.Templates.DefaultConsoleLog,
                theme: AnsiConsoleTheme.Code)
            .WriteTo.MSSqlServer(
                connectionString: auditCnnString,
                sinkOptions: new MSSqlServerSinkOptions()
                {
                    AutoCreateSqlTable = true, 
                    TableName = C8SConstants.LogTables.FunctionsLog, 
                    LevelSwitch = levelSwitch
                })
            .WriteTo.ApplicationInsights(services.GetRequiredService<IConfiguration>()
                .GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"), new TraceTelemetryConverter());
    });

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
