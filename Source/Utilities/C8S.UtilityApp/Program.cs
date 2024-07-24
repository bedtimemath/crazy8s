using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Database.EFCore.Extensions;
using C8S.Database.Repository.Extensions;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Tasks;
using CommandLine;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

/*****************************************
 * INITIAL LOGGING
 */

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

// see: https://jasonterando.medium.com/net-core-console-applications-with-dependency-injection-234eac5a4040
try
{
    /*****************************************
     * BUILDER
     */
    var builder = Host.CreateApplicationBuilder(args);

    /*****************************************
     * LOGGING
     */
    // add some of the configuration so that we can use it below
    var parserResult = Parser.Default
        .ParseArguments<
            LoadC8SDataOptions,
            LoadSampleDataOptions>(args);
    var platform = (parserResult.Value as StandardConsoleOptions)?.Platform ??
                   throw new Exception("Could not cast parser options to StandardConsoleOptions");
    Log.Logger.Warning(platform);
    
    builder.Configuration
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile($"appsettings.{platform}.json", optional: false)
        .AddEnvironmentVariables();

    var host = Host.CreateDefaultBuilder(args)
#if false
        .ConfigureAppConfiguration((bldr) =>
        {
            // add our json file configuration to the context
            //var jsonSource = builder.Configuration.Sources.FirstOrDefault(s => s is JsonConfigurationSource);
            //if (jsonSource != null) bldr.Sources.Add(jsonSource);

            // then add from the app config in Azure
            var cnnString = builder.Configuration.GetConnectionString(C8SConstants.Connections.AppConfig);
            bldr.AddAzureAppConfiguration(config =>
            {
                config.Connect(cnnString)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, platform);
            });
        }) 
#endif
        .ConfigureServices((context, services) =>
        {
            // Parsing the arguments
            parserResult
                .WithParsed<LoadSampleDataOptions>(options =>
                {
                    services.AddSingleton(options);
                    services.AddSingleton<IActionLauncher, LoadSampleData>();
                })
                .WithParsed<LoadC8SDataOptions>(options =>
                {
                    services.AddSingleton(options);
                    services.AddSingleton<IActionLauncher, LoadC8SData>();
                });
            
            /*****************************************
             * AZURE CLIENTS SETUP
             */
            var azureStorageCnnString = builder.Configuration.GetConnectionString(C8SConstants.Connections.AzureStorage);
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(azureStorageCnnString);
            });

            // Setting up the database (new)
            var connectionString = builder.Configuration.GetConnectionString(C8SConstants.Connections.Database);
            services.AddC8SDbContext(connectionString);

            // Set up other services
            services.AddCommonHelpers();
            services.AddC8SRepository();

            var oldSystemCnnString = builder.Configuration.GetConnectionString(C8SConstants.Connections.OldSystem);
            services.AddOldSystemServices(oldSystemCnnString);
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
                theme: AnsiConsoleTheme.Code))
        .Build();

    SelfLog.Enable(m => Console.Error.WriteLine(m));

    var action = host.Services.GetService<IActionLauncher>();
    return (action == null) ? -1 : await action.Launch();
}
catch (Exception ex)
{
    // see: https://githubmate.com/repo/dotnet/runtime/issues/60600
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) { throw; }

    Log.Fatal(ex, "Unhandled exception");
    return -1;
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
