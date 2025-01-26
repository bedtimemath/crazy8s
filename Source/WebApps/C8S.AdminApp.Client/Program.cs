using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Client.Services.Pages;
using C8S.AdminApp.Client.Services.PubSubs;
using C8S.AdminApp.Common;
using C8S.AdminApp.Common.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SC.Common;
using SC.Common.Helpers.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using _ClientImports = C8S.AdminApp.Client._Imports;
using _UIImports = C8S.AdminApp.Client.UI._Imports;
using _ServicesImports = C8S.AdminApp.Client.Services._Imports;

/*****************************************
 * INITIAL LOGGING
 */
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    /*****************************************
     * CONFIGURATION
     */
    // Not applicable on client

    /*****************************************
     * LOGGING
     */
    var levelSwitch = new LoggingLevelSwitch(builder.HostEnvironment.IsDevelopment() ? 
        LogEventLevel.Verbose : LogEventLevel.Warning);
    builder.Services.AddSingleton(levelSwitch);
    
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.ControlledBy(levelSwitch)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("System.Net.Http.HttpClient.BackendServer.ClientHandler", LogEventLevel.Warning)
        .MinimumLevel.Override("System.Net.Http.HttpClient.BackendServer.LogicalHandler", LogEventLevel.Warning)
        .WriteTo.BrowserConsole(
            outputTemplate: SoftCrowConstants.Templates.DefaultConsoleLog)
        .CreateLogger();
    builder.Logging.AddSerilog(Log.Logger);

    /*****************************************
     * AUTHENTICATION
     */
    builder.Services.AddAuthorizationCore();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddAuthenticationStateDeserialization();
    
    /*****************************************
     * MEDIATR
     */
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(_ClientImports).Assembly);
        config.RegisterServicesFromAssembly(typeof(_UIImports).Assembly);
        config.RegisterServicesFromAssembly(typeof(_ServicesImports).Assembly);
    });

    /*****************************************
     * BACKEND REQUESTS
     */
    builder.Services.AddHttpClient(AdminAppConstants.HttpClients.BackendServer, client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    }).RemoveAllLoggers();

    /*****************************************
     * BLAZOR & RADZEN SERVICES
     */
    builder.Services.AddRadzenComponents();
    
    /*****************************************
     * SOFT CROW & LOCAL
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddClientCoordinators();
    
    builder.Services.AddScoped<IPagesService, PagesService>();
    builder.Services.AddScoped<IPubSubService>(services =>
    {
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var busUrl = builder.HostEnvironment.BaseAddress + "communication";
        return new ClientPubSubService(loggerFactory, busUrl);
    });

    /*****************************************
     * APP BUILD & RUN
     */
    await builder.Build().RunAsync();
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
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

