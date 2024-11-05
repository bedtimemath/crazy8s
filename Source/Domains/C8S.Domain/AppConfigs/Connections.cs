namespace C8S.Domain.AppConfigs;

public class Connections
{
    public static string SectionName = nameof(Connections);

    public string AzureStorage { get; set; } = default!;
    public string Database { get; set; } = default!;
}