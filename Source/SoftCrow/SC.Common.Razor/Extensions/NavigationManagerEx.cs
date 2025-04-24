using Microsoft.AspNetCore.Components;

namespace SC.Common.Razor.Extensions;

public static class NavigationManagerEx
{
    public static string GetRelativeUrl(this NavigationManager navigationManager) => 
        navigationManager.ToBaseRelativePath(navigationManager.Uri);
}