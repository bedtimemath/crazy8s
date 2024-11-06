namespace C8S.Domain.AppConfigs;

public class Connections
{
    public static string SectionName = nameof(Connections);
    
    public string? AzureStorage { get; set; }
    public string? Database { get; set; }
    public string? ApplicationInsights { get; set; }
    public string? OldSystem { get; set; }
}