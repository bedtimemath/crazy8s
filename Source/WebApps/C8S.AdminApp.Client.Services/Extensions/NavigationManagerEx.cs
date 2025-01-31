using C8S.AdminApp.Client.Services.Navigation;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationManagerEx
{
    public static string GetRelativeUrl(this NavigationManager navigationManager) => 
        navigationManager.ToBaseRelativePath(navigationManager.Uri);

    public static NavigationGroup? GetGroup(this NavigationManager navigationManager)
    {
        var relativeUrl = GetRelativeUrl(navigationManager);
        var parts = relativeUrl.Split('/');
        return parts[0] switch
        {
            "requests" => NavigationGroup.Requests,
            "contacts" => NavigationGroup.Contacts,
            "sites" => NavigationGroup.Sites,
            "organizations" => NavigationGroup.Organizations,
            "orders" => NavigationGroup.Orders,
            "skus" => NavigationGroup.Skus,
            _ => null
        };
    }
}