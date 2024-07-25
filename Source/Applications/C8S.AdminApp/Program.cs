using Azure.Identity;
using C8S.AdminApp;
using C8S.Common;
using C8S.Common.Helpers.Extensions;
using C8S.Database.Repository.Extensions;
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
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.Is(logLevel)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: SharedConstants.Templates.DefaultConsoleLog,
            theme: AnsiConsoleTheme.Code)
    );
    SelfLog.Enable(m => Console.Error.WriteLine(m));

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

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

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
