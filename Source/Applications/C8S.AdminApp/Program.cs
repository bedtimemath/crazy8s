using Azure.Identity;
using C8S.AdminApp;
using C8S.AdminApp.Services;
using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Common.Models;
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
    var logLevel = builder.Environment.IsProduction() ? LogEventLevel.Information : LogEventLevel.Debug;

    /*****************************************
     * CONFIGURATION
     */

    // check for the two variables we need immediately
    var appConfigCnnString = builder.Configuration["C8S_Admin_AppConfig"];
    var sensitiveFolderPath = builder.Configuration["C8S_Admin_SensitiveFolder"];
    
    if (String.IsNullOrEmpty(sensitiveFolderPath))
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
            .SetBasePath(sensitiveFolderPath)
            .AddJsonFile($"c8s-admin.appsettings.{builder.Environment.EnvironmentName.ToLowerInvariant()}.json", optional: false);
    }

    // load the connections that we need
    var connections = builder.Configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                      throw new Exception($"Missing configuration section: {Connections.SectionName}");

    /*****************************************
     * LOGGING
     */
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

        if (!String.IsNullOrEmpty(connections.ApplicationInsights))
        {
            configuration
                .WriteTo.ApplicationInsights(
                    new TelemetryConfiguration() { ConnectionString = connections.ApplicationInsights }, TelemetryConverter.Traces);
        }
    });
    SelfLog.Enable(m => Console.Error.WriteLine(m));

    /*****************************************
     * APPLICATION INSIGHTS
     */
    if (!String.IsNullOrEmpty(connections.ApplicationInsights))
        builder.Services.AddApplicationInsightsTelemetry();

    /*****************************************
     * AZURE CLIENTS SETUP
     */
    builder.Services.AddAzureClients(clientBuilder =>
    {
        clientBuilder.AddBlobServiceClient(connections.AzureStorage);
    });

    /*****************************************
     * CRAZY 8s SERVICES
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddC8SRepository(connections.Database);
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
