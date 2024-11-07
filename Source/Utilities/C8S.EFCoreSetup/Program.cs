// ************************
// NOTICE: This app is only intended to be used with the EF Core migrations
//    process, as the "startup-project" parameter. The dotnet ef migrations
//    command will use our DbContext injection and the appsettings.json file
//    to perform migrations and scripting work.
// ************************

using System.Diagnostics;
using C8S.Database.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SC.Common;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

//Console.Out.WriteLine("Current ProcessID: " + Process.GetCurrentProcess().Id); //This prints the process id
//Console.Out.WriteLine("Waiting for debugger to attach...");
//while (!Debugger.IsAttached)
//{
//    Thread.Sleep(100);
//}
//Console.Out.WriteLine("Debugger attached!");

var builder = Host.CreateDefaultBuilder(args);

// LOGGING
//    We don't really need logging for this project. However, when the dot ef
//    migrations process runs, it will log to the console automatically. This
//    way we can get rid of a bunch of messages we don't need to see

// add some of the configuration so that we can use it below
builder.ConfigureHostConfiguration(configBuilder =>
{
    configBuilder.AddEnvironmentVariables();
    var config = configBuilder.Build();
    var configFolderPath = config["C8S_ConfigFolder"];

    if (!String.IsNullOrEmpty(configFolderPath))
    {
        configBuilder
            .SetBasePath(configFolderPath)
            .AddJsonFile("c8s.appsettings.development.json", optional: false);
    }
});
    
// set up the serilog logger
builder.UseSerilog((context, services, configuration) => configuration
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: SoftCrowConstants.Templates.DefaultConsoleLog,
        theme: AnsiConsoleTheme.Code)
);
SelfLog.Enable(msg => Debug.WriteLine(msg));

// CONTEXT SETUP
builder.ConfigureServices((context, services) =>
{
    // LIBRARY
    var libraryConnection = context.Configuration["Connections:Database"];
    services.AddDbContext<C8SDbContext>(config => 
        config.UseSqlServer(libraryConnection));
});

// BUILD & RUN
var app = builder.Build();
app.Run();
