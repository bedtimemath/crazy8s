using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class DomainUrlEx
{
    public static DomainEntity? GetDomainEntityFromUrl(string relativeUrl)
    {
        var url = TrimUrl(relativeUrl);
        if (String.IsNullOrWhiteSpace(url)) return null;

        var parts = relativeUrl.Split('/');
        if (String.IsNullOrWhiteSpace(parts[0])) return null;

        return parts[0] switch
        {
            "contacts" => DomainEntity.Contact,
            "orders" => DomainEntity.Order,
            "organizations" => DomainEntity.Organization,
            "requests" => DomainEntity.Request,
            "sites" => DomainEntity.Site,
            "skus" => DomainEntity.Sku,
            "tickets" => DomainEntity.Ticket,
            _ => null
        };
    }

    public static int? GetIdValueFromUrl(string relativeUrl)
    {
        var url = TrimUrl(relativeUrl);
        if (String.IsNullOrWhiteSpace(url)) return null;

        var parts = relativeUrl.Split('/');
        if (parts.Length < 2) return null;

        if (!Int32.TryParse(parts[1], out var idValue)) return null;
        return idValue;
    }

    public static string CreateUrlFromEntityIdValue(MenuItem menuItem) =>
        CreateUrlFromEntityIdValue(menuItem.Entity, menuItem.IdValue);
    public static string CreateUrlFromEntityIdValue(DomainEntity entity, int idValue) =>
        entity switch
        {
            DomainEntity.Contact => $"contacts/{idValue}",
            DomainEntity.Order => $"orders/{idValue}",
            DomainEntity.Organization => $"organizations/{idValue}",
            DomainEntity.Request => $"requests/{idValue}",
            DomainEntity.Site => $"sites/{idValue}",
            DomainEntity.Sku => $"skus/{idValue}",
            DomainEntity.Ticket => $"tickets/{idValue}",
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
        };

    private static string? TrimUrl(string? url)
    {
        url = url?.Trim();
        if (url?.StartsWith('/') ?? false) url = url?[1..];
        if ((url?.IndexOf('?') ?? -1) >= 0) url = url?[..url.IndexOf('?')];

        return url;
    }
}