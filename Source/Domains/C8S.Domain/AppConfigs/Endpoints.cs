namespace C8S.Domain.AppConfigs;

public class Endpoints
{
    public static string SectionName = nameof(Endpoints);

    public string? AzureStorage { get; set; }
    public string? FullSlateApi { get; set; }
    public string? C8SAdminApp { get; set; }
    public string? WPCoachesAreaApi { get; set; }
}