using Blazr.RenderState.WASM;
using C8S.AdminApp.Client.Auth;
using C8S.AdminApp.Client.Services;
using C8S.AdminApp.Client.Services.Callbacks;
using C8S.AdminApp.Client.Services.Extensions;
using C8S.AdminApp.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SC.Common.Helpers.Extensions;
using Serilog;
using Serilog.Core;
using _ClientImports = C8S.AdminApp.Client._Imports;
using _UIImports = C8S.AdminApp.Client.UI._Imports;
using _ServicesImports = C8S.AdminApp.Client.Services._Imports;

/*****************************************
 * INITIAL LOGGING
 */
// for WASM see: https://stackoverflow.com/questions/71220619/use-serilog-as-logging-provider-in-blazor-webassembly-client-app?rq=1
var levelSwitch = new LoggingLevelSwitch();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(levelSwitch)
    .WriteTo.BrowserConsole()
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
    // Not applicable on client

    /*****************************************
     * AUTHENTICATION
     */
    builder.Services.AddAuthorizationCore();
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
    
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
    });

    /*****************************************
     * BLAZOR & RADZEN SERVICES
     */
    builder.Services.AddRadzenComponents();
    builder.AddBlazrRenderStateWASMServices();
    
    /*****************************************
     * SOFT CROW & LOCAL
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddClientServices();

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

