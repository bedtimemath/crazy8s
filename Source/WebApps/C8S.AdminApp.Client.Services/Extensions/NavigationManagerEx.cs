using C8S.AdminApp.Client.Services.Navigation.Enums;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationManagerEx
{
    public static string GetRelativeUrl(this NavigationManager navigationManager) => 
        navigationManager.ToBaseRelativePath(navigationManager.Uri);

    public static NavigationEntity? GetGroup(this NavigationManager navigationManager)
    {
        var relativeUrl = GetRelativeUrl(navigationManager);
        var parts = relativeUrl.Split('/');
        return parts[0] switch
        {
            "requests" => NavigationEntity.Requests,
            "contacts" => NavigationEntity.Contacts,
            "sites" => NavigationEntity.Sites,
            "organizations" => NavigationEntity.Organizations,
            "orders" => NavigationEntity.Orders,
            "skus" => NavigationEntity.Skus,
            _ => null
        };
    }
}