using C8S.Domain.Features;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class StringEx
{
    public static DomainEntity? ToDomainEntity(this string relativeUrl)
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
    public static int? ToIdValue(this string relativeUrl)
    {
        var url = TrimUrl(relativeUrl);
        if (String.IsNullOrWhiteSpace(url)) return null;

        var parts = relativeUrl.Split('/');
        if (parts.Length < 2) return null;

        if (!Int32.TryParse(parts[1], out var idValue)) return null;
        return idValue;
    }

    private static string? TrimUrl(string? url)
    {
        url = url?.Trim();
        if (url?.StartsWith('/') ?? false) url = url?[1..];
        if ((url?.IndexOf('?') ?? -1) >= 0) url = url?[..url.IndexOf('?')];

        return url;
    }
}