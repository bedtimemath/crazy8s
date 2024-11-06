namespace C8S.Domain.AppConfigs;

public class Endpoints
{
    public static string SectionName = nameof(Endpoints);

    public string? AzureStorage { get; set; }
    public string? FullSlateApi { get; set; }
}