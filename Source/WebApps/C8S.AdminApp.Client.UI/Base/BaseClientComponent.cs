using C8S.AdminApp.Client.Services.Data;
using C8S.AdminApp.Client.Services.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Base;

public abstract class BaseClientComponent: BaseRazorComponent
{
    #region Injected Properties
    [Inject]
    public ILogger<BaseClientComponent> BaseLogger { get; set; } = null!;

    [Inject]
    public IServiceProvider ServiceProvider { get; set; } = null!; 
    #endregion
    
    #region Protected Properties
    protected IPagesService? PagesService { get; set; } = null;
    protected ICommunicationService? CommunicationService { get; set; } = null;
    #endregion
    
    #region Component LifeCycle

    protected override void OnInitialized()
    {
        base.OnInitialized();

        PagesService = ServiceProvider.GetService<IPagesService>();
        CommunicationService = ServiceProvider.GetService<ICommunicationService>();
    }
    #endregion
}