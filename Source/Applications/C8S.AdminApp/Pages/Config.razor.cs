using C8S.Common.Models;
using C8S.Common.Razor.Base;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Pages;

public partial class Config : BaseRazorPage
{
    [Inject]
    public ILogger<Config> Logger { get; set; } = default!;

    [Inject]
    public IConfiguration ConfigManager { get; set; } = default!;

    private Connections _connections = default!;
    //private Endpoints _endpoints = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _connections = ConfigManager.GetSection(Connections.SectionName).Get<Connections>() ??
                       throw new Exception($"Could not read section: {Connections.SectionName}");
        //_endpoints = ConfigManager.GetSection(Endpoints.SectionName).Get<Endpoints>() ??
        //             throw new Exception($"Could not read section: {Endpoints.SectionName}");
    }
}