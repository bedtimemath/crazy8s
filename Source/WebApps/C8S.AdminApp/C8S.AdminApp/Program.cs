using Azure.Identity;
using Blazr.RenderState.Server;
using C8S.AdminApp;
using C8S.AdminApp.Services;
using C8S.Domain.AppConfigs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using SC.Common;
using SC.Common.Helpers.Extensions;
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

try
{
    var builder = WebApplication.CreateBuilder(args);
    var logLevel = builder.Environment.IsProduction() ? LogEventLevel.Information : LogEventLevel.Debug;

    /*****************************************
     * CONFIGURATION
     */
    builder.Configuration.AddEnvironmentVariables();

    var appConfigCnnString = builder.Configuration["C8S_AppConfig"];
    var configFolderPath = builder.Configuration["C8S_ConfigFolder"];

    if (String.IsNullOrEmpty(configFolderPath))
    {
        // configure with the azure configuration
        builder.Configuration
            .AddAzureAppConfiguration(config =>
            {
                config.Connect(appConfigCnnString)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
            });
    }
    else
    {
        // configure with a file (much faster)
        builder.Configuration
            .SetBasePath(configFolderPath)
            .AddJsonFile($"c8s.appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: false);
    }

    // load the connections that we need
    var connections = builder.Configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                      throw new Exception($"Missing configuration section: {Connections.SectionName}");

    /*****************************************
     * LOGGING
     */
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.Is(logLevel)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: SharedConstants.Templates.DefaultConsoleLog,
            theme: AnsiConsoleTheme.Code)
    );
    SelfLog.Enable(m => Console.Error.WriteLine(m));

    /*****************************************
     * MINIMAL APIS
     */
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    /*****************************************
     * SIGNAL-R
     */
    builder.Services.AddSignalR();
    builder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            ["application/octet-stream"]);
    });

    /*****************************************
     * SOFT CROW & LOCAL
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddScoped<SelfService>();

    /*****************************************
     * API CONTROLLERS
     */
    builder.Services.AddControllers();

    /*****************************************
     * BLAZOR SERVICES
     */
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();

    // for checking the render state
    builder.AddBlazrRenderStateServerServices();

    /*****************************************
     * APP
     */
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    //app.MapHub<ChatHub>("/changes");

    //app.MapAccountLoginLogout();
    //app.MapApiEndpoints();
    app.MapControllers();

    app.MapRazorComponents<AppRoot>()
        .AddInteractiveServerRenderMode()
        .AddAdditionalAssemblies(
            typeof(C8S.AdminApp.Client._Imports).Assembly,
            typeof(SC.Common.Razor._Imports).Assembly);

    app.Run();

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

