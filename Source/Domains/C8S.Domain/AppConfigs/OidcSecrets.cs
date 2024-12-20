namespace C8S.Domain.AppConfigs;

public class OidcSecrets
{
    public static string SectionName = nameof(OidcSecrets);

    public string Authority { get; set; } = null!;
    public string? ClientId { get; set; }
}