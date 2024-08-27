using System.Reflection;
using Azure.Identity;
using C8S.Common;
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

try
{
    var host = new HostBuilder()
        .ConfigureHostConfiguration(builder =>
        {
            Log.Logger.Information("Configuring Host Configuration");

            builder.AddJsonFile("local.settings.json", true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables();
        })
        .ConfigureAppConfiguration((hostContext, builder) =>
        {
            //Log.Logger.Information("Configuring App Configuration");

            var cnnString = hostContext.Configuration.GetConnectionString(C8SConstants.Connections.AppConfig) ??
                            hostContext.Configuration[$"ConnectionStrings:{C8SConstants.Connections.AppConfig}"];
            var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            builder.AddAzureAppConfiguration(config =>
            {
                Log.Logger.Information("Connecting to Azure App Configuration using: {cnnString}", cnnString);
                config.Connect(cnnString)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, environmentName);
            });
        })
        .ConfigureFunctionsWebApplication()
        .ConfigureServices((hostContext, services) =>
        {
            Log.Logger.Information("Configuring Services");

            /*****************************************
             * AZURE CLIENTS SETUP
             */
            var azureStorageCnnString = hostContext.Configuration.GetConnectionString(C8SConstants.Connections.AzureStorage) ??
                                        hostContext.Configuration[$"ConnectionStrings:{C8SConstants.Connections.AzureStorage}"];
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddQueueServiceClient(azureStorageCnnString);
                clientBuilder.AddBlobServiceClient(azureStorageCnnString);
            });

            /*****************************************
             * REPOSITORY SETUP
             */
            //var connectionString = hostContext.Configuration.GetConnectionString(C8SConstants.Connections.Database);
            //services.AddC8SDbContext(connectionString);

            /*****************************************
             * OTHER LIBRARY SETUP
             */
            //services.AddCommonHelpers();
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
        })
        .UseSerilog((context, services, config) => config
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
        )
        .Build();

    SelfLog.Enable(m => Console.Error.WriteLine(m));

    host.Run();
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
