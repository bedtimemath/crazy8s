using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SC.Common.Radzen.Base;

namespace C8S.AdminApp.Client.Pages;

[AllowAnonymous]
public partial class Anonymous: BaseRazorPage
{
    #region Injected Properties
    [Inject]
    public ILogger<Anonymous> Logger { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Event Handlers

    private void HandleLogInClicked()
    {
        NavigationManager.NavigateTo("/authentication/login/?returnUrl=/home", true);
    }
    #endregion
}