using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationManagerEx
{
    public static string GetRelativeUrl(this NavigationManager navigationManager) => 
        navigationManager.ToBaseRelativePath(navigationManager.Uri);
}