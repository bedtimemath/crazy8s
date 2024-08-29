using Azure.Identity;
using C8S.Applications.Extensions;
using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Common.Models;
using C8S.Database.Repository.Extensions;
using C8S.UtilityApp.Base;
using C8S.UtilityApp.Extensions;
using C8S.UtilityApp.Tasks;
using CommandLine;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
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
     * PARSER
     */
    var parserResult = Parser.Default
        .ParseArguments<
            LoadC8SDataOptions,
            LoadSampleDataOptions,
            ProcessApplicationsOptions,
            ShowConfigOptions>(args);
    var platform = (parserResult.Value as StandardConsoleOptions)?.Platform ??
                   throw new Exception("Could not cast parser options to StandardConsoleOptions");
    Log.Logger.Warning(platform);

    /*****************************************
     * CONFIGURATION
     */
    var configBuilder = builder.Configuration
        .AddEnvironmentVariables();

    // check for the two variables we need immediately
    var appConfigCnnString = builder.Configuration["C8S_Admin_AppConfig"];
    var sensitiveFolderPath = builder.Configuration["C8S_Admin_SensitiveFolder"];

    if (String.IsNullOrEmpty(sensitiveFolderPath))
    {
        // configure with the azure configuration
        configBuilder = configBuilder
            .AddAzureAppConfiguration(config =>
            {
                config.Connect(appConfigCnnString)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, platform);
            });
    }
    else
    {
        // configure with a file (much faster)
        configBuilder = configBuilder
            .SetBasePath(sensitiveFolderPath)
            .AddJsonFile($"c8s-admin.appsettings.{platform.ToLowerInvariant()}.json", optional: false);
    }

    var configuration = configBuilder.Build();

    // load the connections that we need
    var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                      throw new Exception($"Missing configuration section: {Connections.SectionName}");

    /*****************************************
     * HOST SETUP
     */
    var hostBuilder = Host.CreateDefaultBuilder(args);

    /*****************************************
     * CONFIGURE SERVICES
     */
    hostBuilder.ConfigureServices((context, services) =>
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
            })
            .WithParsed<ProcessApplicationsOptions>(options =>
            {
                services.AddSingleton(options);
                services.AddSingleton<IActionLauncher, ProcessApplications>();
            })
            .WithParsed<ShowConfigOptions>(options =>
            {
                services.AddSingleton(options);
                services.AddSingleton<IActionLauncher, ShowConfig>();
            });

        services.AddSingleton<IConfiguration>(configuration);

        /*****************************************
         * AZURE CLIENTS SETUP
         */
        services.AddAzureClients(clientBuilder =>
        {
            clientBuilder.AddBlobServiceClient(connections.AzureStorage);
        });

        /*****************************************
         * C8S SERVICES
         */
        if (String.IsNullOrEmpty(connections.OldSystem))
            throw new Exception("Missing OldSystem connection string");

        services.AddCommonHelpers();
        services.AddC8SRepository(connections.Database);
        services.AddOldSystemServices(connections.OldSystem);
        services.AddApplicationServices();
    });

    /*****************************************
     * LOGGING
     */
    hostBuilder.UseSerilog((context, services, config) => config
            .MinimumLevel.Is(LogEventLevel.Verbose)
            .MinimumLevel.Override("Azure.Core", LogEventLevel.Warning)
            .MinimumLevel.Override("EntityFrameworkCore.Triggered", LogEventLevel.Warning)
            .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: SharedConstants.Templates.DefaultConsoleLog,
                theme: AnsiConsoleTheme.Code));

    SelfLog.Enable(m => Console.Error.WriteLine(m));

    /*****************************************
     * RUN
     */
    var host = hostBuilder.Build();
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
