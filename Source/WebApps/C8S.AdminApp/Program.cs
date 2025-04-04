using Azure.Identity;
using C8S.AdminApp;
using C8S.AdminApp.Auth;
using C8S.AdminApp.Extensions;
using C8S.AdminApp.Services;
using C8S.Domain.AppConfigs;
using C8S.Domain.EFCore.Extensions;
using C8S.Domain.Mapping.Extensions;
using C8S.FullSlate.Extensions;
using C8S.WordPress.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Radzen;
using SC.Audit.EFCore.Extensions;
using SC.Common;
using SC.Common.Helpers.Extensions;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
#if !DEBUG
using C8S.Domain;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using Serilog.Sinks.MSSqlServer;
#endif

/*****************************************
 * INITIAL LOGGING
 */
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

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
    var oidcSecrets = builder.Configuration.GetSection(OidcSecrets.SectionName).Get<OidcSecrets>() ??
                      throw new Exception($"Missing configuration section: {OidcSecrets.SectionName}");
    var apiKeys = builder.Configuration.GetSection(ApiKeys.SectionName).Get<ApiKeys>() ??
                  throw new Exception($"Missing configuration section: {ApiKeys.SectionName}");
    var endpoints = builder.Configuration.GetSection(Endpoints.SectionName).Get<Endpoints>() ??
                    throw new Exception($"Missing configuration section: {Endpoints.SectionName}");

    /*****************************************
     * LOGGING
     */
    var levelSwitch = new LoggingLevelSwitch(builder.Environment.IsDevelopment() ?
        LogEventLevel.Verbose : LogEventLevel.Warning);
    builder.Services.AddSingleton(levelSwitch);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .MinimumLevel.ControlledBy(levelSwitch)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .Enrich.FromLogContext()
#if !DEBUG
        .WriteTo.MSSqlServer(
            connectionString: connections.Audit,
            sinkOptions: new MSSqlServerSinkOptions()
            {
                AutoCreateSqlTable = true, 
                TableName = C8SConstants.LogTables.AdminLog, 
                LevelSwitch = levelSwitch
            })
        .WriteTo.ApplicationInsights(services.GetRequiredService<IConfiguration>()
            .GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"), 
            telemetryConverter: new TraceTelemetryConverter())
#endif    
        .WriteTo.Console(
            outputTemplate: SoftCrowConstants.Templates.DefaultConsoleLog,
            theme: AnsiConsoleTheme.Code));
    SelfLog.Enable(m => Console.Error.WriteLine(m));

    /*****************************************
     * AZURE CLIENTS SETUP
     */
    builder.Services.AddAzureClients(clientBuilder =>
    {
        clientBuilder.AddBlobServiceClient(connections.AzureStorage);
        clientBuilder.AddQueueServiceClient(connections.AzureStorage);
    });

    /*****************************************
     * AUTOMAPPER
     */
    builder.Services.AddDomainMapping();

    /*****************************************
     * SOFT CROW & LOCAL
     */
    builder.Services.AddCommonHelpers();
    builder.Services.AddSCAuditContext(connections.Audit);
    builder.Services.AddC8SDbContext(connections.Database);

    if (String.IsNullOrEmpty(endpoints.FullSlateApi)) throw new Exception("Missing Endpoints:FullSlateApi");
    if (String.IsNullOrEmpty(apiKeys.FullSlate)) throw new Exception("Missing ApiKeys:FullSlate");
    builder.Services.AddFullSlateServices(endpoints.FullSlateApi, apiKeys.FullSlate);

    if (String.IsNullOrEmpty(endpoints.WPCoachesAreaApi)) throw new Exception("Missing Endpoints:WPCoachesAreaApi");
    if (String.IsNullOrEmpty(apiKeys.WPCoachesArea)) throw new Exception("Missing ApiKeys:WPCoachesArea");
    builder.Services.AddWordPressServices(endpoints.WPCoachesAreaApi, apiKeys.WPCoachesArea);

    builder.Services.AddScoped<ISelfService, SelfService>();

    /*****************************************
     * MINIMAL APIS
     */
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();

    /*****************************************
     * SIGNAL-R
     */
    builder.Services.AddSignalRServices();

    /*****************************************
     * API CONTROLLERS
     */
    builder.Services.AddControllers();

    /*****************************************
     * AUTHENTICATION
     */
    builder.Services.AddAuthentication(SoftCrowConstants.OidcSchemes.Microsoft)
        .AddOpenIdConnect(SoftCrowConstants.OidcSchemes.Microsoft, oidcOptions =>
        {
            // For the following OIDC settings, any line that's commented out
            // represents a DEFAULT setting. If you adopt the default, you can
            // remove the line if you wish.

            // ........................................................................
            // The OIDC handler must use a sign-in scheme capable of persisting 
            // user credentials across requests.

            oidcOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            // ........................................................................

            // ........................................................................
            // The "openid" and "profile" scopes are required for the OIDC handler 
            // and included by default. You should enable these scopes here if scopes 
            // are provided by "Authentication:Schemes:MicrosoftOidc:Scope" 
            // configuration because configuration may overwrite the scopes collection.

            //oidcOptions.Scope.Add(OpenIdConnectScope.OpenIdProfile);
            // ........................................................................
            oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);

            // ........................................................................
            // The following paths must match the redirect and post logout redirect 
            // paths configured when registering the application with the OIDC provider. 
            // For Microsoft Entra ID, this is accomplished through the "Authentication" 
            // blade of the application's registration in the Azure portal. Both the
            // signin and signout paths must be registered as Redirect URIs. The default 
            // values are "/signin-oidc" and "/signout-callback-oidc".
            // Microsoft Identity currently only redirects back to the 
            // SignedOutCallbackPath if authority is 
            // https://login.microsoftonline.com/{TENANT ID}/v2.0/ as it is above. 
            // You can use the "common" authority instead, and logout redirects back to 
            // the Blazor app. For more information, see 
            // https://github.com/AzureAD/microsoft-authentication-library-for-js/issues/5783

            //oidcOptions.CallbackPath = new PathString("/signin-oidc");
            //oidcOptions.SignedOutCallbackPath = new PathString("/signout-callback-oidc");
            // ........................................................................

            // ........................................................................
            // The RemoteSignOutPath is the "Front-channel logout URL" for remote single 
            // sign-out. The default value is "/signout-oidc".

            //oidcOptions.RemoteSignOutPath = new PathString("/signout-oidc");
            // ........................................................................

            // ........................................................................
            // The following example Authority is configured for Microsoft Entra ID
            // and a single-tenant application registration. Set the {TENANT ID} 
            // placeholder to the Tenant ID. The "common" Authority 
            // https://login.microsoftonline.com/common/v2.0/ should be used 
            // for multi-tenant apps. You can also use the "common" Authority for 
            // single-tenant apps, but it requires a custom IssuerValidator as shown 
            // in the comments below. 

            oidcOptions.Authority = oidcSecrets.Authority;
            // ........................................................................

            // ........................................................................
            // Set the Client ID for the app. Set the {CLIENT ID} placeholder to
            // the Client ID.

            oidcOptions.ClientId = oidcSecrets.ClientId;
            // ........................................................................

            // ........................................................................
            // Setting ResponseType to "code" configures the OIDC handler to use 
            // authorization code flow. Implicit grants and hybrid flows are unnecessary
            // in this mode. In a Microsoft Entra ID app registration, you don't need to 
            // select either box for the authorization endpoint to return access tokens 
            // or ID tokens. The OIDC handler automatically requests the appropriate 
            // tokens using the code returned from the authorization endpoint.

            oidcOptions.ResponseType = OpenIdConnectResponseType.Code;
            // ........................................................................

            // ........................................................................
            // Set MapInboundClaims to "false" to obtain the original claim types from 
            // the token. Many OIDC servers use "name" and "role"/"roles" rather than 
            // the SOAP/WS-Fed defaults in ClaimTypes. Adjust these values if your 
            // identity provider uses different claim types.

            oidcOptions.MapInboundClaims = false;
            oidcOptions.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
            oidcOptions.TokenValidationParameters.RoleClaimType = "roles";
            // ........................................................................

            // ........................................................................
            // Many OIDC providers work with the default issuer validator, but the
            // configuration must account for the issuer parameterized with "{TENANT ID}" 
            // returned by the "common" endpoint's /.well-known/openid-configuration
            // For more information, see
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1731

            //var microsoftIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(oidcOptions.Authority);
            //oidcOptions.TokenValidationParameters.IssuerValidator = microsoftIssuerValidator.Validate;
            // ........................................................................

            // ........................................................................
            // OIDC connect options set later via ConfigureCookieOidcRefresh
            //
            // (1) The "offline_access" scope is required for the refresh token.
            //
            // (2) SaveTokens is set to true, which saves the access and refresh tokens
            // in the cookie, so the app can authenticate requests for weather data and
            // use the refresh token to obtain a new access token on access token
            // expiration.
            // ........................................................................
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

    // ConfigureCookieOidcRefresh attaches a cookie OnValidatePrincipal callback to get
    // a new access token when the current one expires, and reissue a cookie with the
    // new access token saved inside. If the refresh fails, the user will be signed
    // out. OIDC connect options are set for saving tokens and the offline access
    // scope.
    builder.Services.ConfigureCookieOidcRefresh(
        CookieAuthenticationDefaults.AuthenticationScheme,
        SoftCrowConstants.OidcSchemes.Microsoft);
    builder.Services.AddAuthorization();

    builder.Services.AddHttpContextAccessor();

    /*****************************************
     * RADZEN SERVICES
     */
    builder.Services.AddRadzenComponents();

    /*****************************************
     * BLAZOR SERVICES
     */
    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents()
        .AddAuthenticationStateSerialization();

    // currently required for login / logout
    builder.Services.AddRazorPages();

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
    app.UseAntiforgery();
    app.UseSignalRServices();

    app.MapControllers();

    app.MapStaticAssets();
    app.MapRazorComponents<AppRoot>()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(
            typeof(C8S.AdminApp.Client._Imports).Assembly,
            typeof(SC.Common.Razor._Imports).Assembly);

    app.MapGroup("/authentication").MapLoginAndLogout();

    // currently required for login / logout
    app.MapRazorPages();

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

