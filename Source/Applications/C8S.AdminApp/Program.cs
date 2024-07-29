using Azure.Identity;
using C8S.AdminApp;
using C8S.AdminApp.Services;
using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Database.Repository.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Radzen;
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
    /*****************************************
     * BUILDER
     */
    var builder = WebApplication.CreateBuilder(args);

    /*****************************************
     * CONFIGURATION
     */
    // We use this simple distinguishing variable to determine the log level
    //	and whether to set our GA to debug (below)
    var logLevel = builder.Environment.IsProduction() ? LogEventLevel.Information : LogEventLevel.Debug;

    var appInsightsConnection = !builder.Environment.IsProduction() ? null :
        "InstrumentationKey=5e247dc2-1787-4a48-a65b-27195490fa48;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/;ApplicationId=ea002bd8-896a-4f82-a734-aa71b9826bf8";

    /*****************************************
     * LOGGING
     */
    // add some of the configuration so that we can use it below
    builder.Configuration
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile($"appsettings.json", optional: true)
        .AddEnvironmentVariables();

    // using the app settings json file during development is much faster
    if (!builder.Environment.IsDevelopment() || !File.Exists($"appsettings.json"))
    {
        builder.Configuration
            .AddAzureAppConfiguration(config =>
            {
                var cnnString = builder.Configuration.GetConnectionString(C8SConstants.Connections.AppConfig);
                var environmentName = builder.Environment.EnvironmentName;

                config.Connect(cnnString)
                    .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()))
                    .Select(KeyFilter.Any, LabelFilter.Null)
                    .Select(KeyFilter.Any, environmentName);
            });
    }

    // set up the serilog logger
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .MinimumLevel.Is(logLevel)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: SharedConstants.Templates.DefaultConsoleLog,
                theme: AnsiConsoleTheme.Code);

        if (!String.IsNullOrEmpty(appInsightsConnection))
        {
            configuration
                .WriteTo.ApplicationInsights(
                    new TelemetryConfiguration() { ConnectionString = appInsightsConnection }, TelemetryConverter.Traces);
        }
    });
    SelfLog.Enable(m => Console.Error.WriteLine(m));

    /*****************************************
     * APPLICATION INSIGHTS
     */
    if (!String.IsNullOrEmpty(appInsightsConnection))
        builder.Services.AddApplicationInsightsTelemetry();

    /*****************************************
     * AZURE CLIENTS SETUP
     */
    var azureStorageCnnString = builder.Configuration.GetConnectionString(C8SConstants.Connections.AzureStorage);
    builder.Services.AddAzureClients(clientBuilder =>
    {
        clientBuilder.AddBlobServiceClient(azureStorageCnnString);
    });

    /*****************************************
     * CRAZY 8s SERVICES
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddC8SRepository();
    builder.Services.AddSingleton<SelfService>();
    
    /*****************************************
     * RADZEN SERVICES
     */
    builder.Services.AddScoped<DialogService>();
    builder.Services.AddScoped<NotificationService>();
    builder.Services.AddScoped<TooltipService>();
    builder.Services.AddScoped<ContextMenuService>();

    /*****************************************
     * WEBSITE SETUP
     */
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();

    /*****************************************
     * APP
     */
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<AppRoot>()
        .AddInteractiveServerRenderMode();
    
    // Use middleware to check the Easy Auth header.
    app.Use(async (context, next) =>
    {
        var selfService = context.RequestServices.GetRequiredService<SelfService>();
        var username = (context?.Request.Headers
                .FirstOrDefault(kvp => kvp.Key == "X-MS-CLIENT-PRINCIPAL-NAME").Value)?
                .ToString();

        if (!String.IsNullOrEmpty(username))
            selfService.SetUsername(username!);

        await next(context!);
    }); 

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
