using System.Reflection;
using Blazr.RenderState.WASM;
using C8S.AdminApp.Client;
using C8S.AdminApp.Client.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using SC.Common.Helpers.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddCommonHelpers();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.AddBlazrRenderStateWASMServices();

builder.Services.AddRadzenComponents();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(_Imports).Assembly));


await builder.Build().RunAsync();
