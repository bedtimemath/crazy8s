namespace C8S.Domain.AppConfigs;

public class Auth0Secrets
{
    public static string SectionName = nameof(Auth0Secrets);

    /// <summary>
    /// The default or custom root domain for your Auth0 tenant. 
    /// </summary>
    public string Domain { get; set; } = null!;
    /// <summary>
    /// The Client ID of the Auth0 Machine-to-Machine application.
    /// </summary>
    public string? ClientId { get; set; }
    /// <summary>
    /// The Client Secret of the Auth0 Machine-to-Machine application.
    /// </summary>
    public string? ClientSecret { get; set; }
}