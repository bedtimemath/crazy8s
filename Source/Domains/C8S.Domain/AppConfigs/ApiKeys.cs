namespace C8S.Domain.AppConfigs;

public class ApiKeys
{
    public static string SectionName = nameof(ApiKeys);

    public string? FullSlate { get; set; }
    public string? C8SAdmin { get; set; }
    public string? WPCoachesArea { get; set; }
}