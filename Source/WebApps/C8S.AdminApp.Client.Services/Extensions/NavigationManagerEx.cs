using C8S.Domain.Features;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationManagerEx
{
    public static string GetRelativeUrl(this NavigationManager navigationManager) => 
        navigationManager.ToBaseRelativePath(navigationManager.Uri);

    public static DomainEntity? GetGroup(this NavigationManager navigationManager)
    {
        var relativeUrl = GetRelativeUrl(navigationManager);
        var parts = relativeUrl.Split('/');
        return parts[0] switch
        {
            "requests" => DomainEntity.Request,
            "contacts" => DomainEntity.Contact,
            "sites" => DomainEntity.Site,
            "organizations" => DomainEntity.Organization,
            "orders" => DomainEntity.Order,
            "skus" => DomainEntity.Sku,
            _ => null
        };
    }
}