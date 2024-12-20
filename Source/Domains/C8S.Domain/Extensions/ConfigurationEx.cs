using System.Text;
using C8S.Domain.AppConfigs;
using Microsoft.Extensions.Configuration;
using SC.Common.Extensions;

namespace C8S.Domain.Extensions;

public static class ConfigurationEx
{
    public static string CreatePingOutput(this IConfiguration configuration,
        params KeyValuePair<string, string>[] extraItems)
    {
        var sbOutput = new StringBuilder();

        // GENERAL
        sbOutput.Append("== General ==\r\n");
        sbOutput.AppendFormat("Environment: {0}\r\n", configuration["ENVIRONMENT"]);
        foreach (var extraItem in (extraItems ?? []))
            sbOutput.AppendFormat("{0}: {1}\r\n", extraItem.Key, extraItem.Value);

        // API KEYS
        var apiKeys = configuration.GetSection(ApiKeys.SectionName).Get<ApiKeys>() ??
                      throw new Exception($"Missing configuration section: {ApiKeys.SectionName}");
        sbOutput.Append("== ApiKeys ==\r\n");
        sbOutput.AppendFormat("FullSlate: {0}\r\n", apiKeys.FullSlate?.Obscure());
        sbOutput.AppendFormat("C8SAdmin: {0}\r\n", apiKeys.C8SAdmin?.Obscure());
        
        // CONNECTIONS
        var connections = configuration.GetSection(Connections.SectionName).Get<Connections>() ??
                          throw new Exception($"Missing configuration section: {Connections.SectionName}");
        sbOutput.Append("== Connections ==\r\n");
        sbOutput.AppendFormat("ApplicationInsights: {0}\r\n", connections.ApplicationInsights?.Obscure());
        sbOutput.AppendFormat("Audit: {0}\r\n", connections.Audit?.Obscure());
        sbOutput.AppendFormat("AzureStorage: {0}\r\n", connections.AzureStorage?.Obscure());
        sbOutput.AppendFormat("Database: {0}\r\n", connections.Database?.Obscure());
        sbOutput.AppendFormat("OldSystem: {0}\r\n", connections.OldSystem?.Obscure());

        // ENDPOINTS
        var endpoints = configuration.GetSection(Endpoints.SectionName).Get<Endpoints>() ??
                      throw new Exception($"Missing configuration section: {Endpoints.SectionName}");
        sbOutput.Append("== Endpoints ==\r\n");
        sbOutput.AppendFormat("AzureStorage: {0}\r\n", endpoints.AzureStorage?.Obscure());
        sbOutput.AppendFormat("FullSlateApi: {0}\r\n", endpoints.FullSlateApi?.Obscure());
        sbOutput.AppendFormat("C8SAdminApp: {0}\r\n", endpoints.C8SAdminApp?.Obscure());
        
        // OIDC SECRETS
        var oidcSecrets = configuration.GetSection(OidcSecrets.SectionName).Get<OidcSecrets>() ??
                      throw new Exception($"Missing configuration section: {OidcSecrets.SectionName}");
        sbOutput.Append("== OidcSecrets ==\r\n");
        sbOutput.AppendFormat("Authority: {0}\r\n", oidcSecrets.Authority?.Obscure());
        sbOutput.AppendFormat("ClientId: {0}\r\n", oidcSecrets.ClientId?.Obscure());

        return sbOutput.ToString();
    }
}