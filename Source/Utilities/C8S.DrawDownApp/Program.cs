using System.Net.Http.Headers;
using C8S.DrawDownApp.Base;
using C8S.DrawDownApp.Models;
using C8S.DrawDownApp.Tasks;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

const string defaultConsoleLog =
    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

/*****************************************
* INITIAL LOGGING
*/

Log.Logger = new LoggerConfiguration()
.WriteTo.Console()
.CreateBootstrapLogger();

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
            RemoveC8SDataOptions>(args);
    var platform = (parserResult.Value as StandardConsoleOptions)?.Platform ??
                   throw new Exception("Could not cast parser options to StandardConsoleOptions");
    Log.Logger.Warning(platform);
    
    /*****************************************
     * CONFIGURATION
     */
    var configBuilder = builder.Configuration
        .AddEnvironmentVariables();

    // check for the two variables we need immediately
    var appConfig = builder.Configuration["C8S_AppConfig"];
    var configFolder = builder.Configuration["C8S_ConfigFolder"];

    if (String.IsNullOrEmpty(configFolder))
        throw new Exception("Missing configuration folder.");
    else
    {
        // configure with a file (much faster)
        configBuilder = configBuilder
            .SetBasePath(configFolder)
            .AddJsonFile($"c8s.appsettings.{platform.ToLowerInvariant()}.json", optional: false);
    }

    var configuration = configBuilder.Build(); 

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
            .WithParsed<RemoveC8SDataOptions>(options =>
            {
                services.AddSingleton(options);
                services.AddSingleton<IActionLauncher, RemoveC8SData>();
            });

        services.AddSingleton<IConfiguration>(configuration);
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
                outputTemplate: defaultConsoleLog,
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
