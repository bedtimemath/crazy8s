namespace C8S.DrawDownApp.Models;

public class Connections
{
    public static string SectionName = nameof(Connections);
    
    public string? Database { get; set; }
    public string? OldSystem { get; set; }
}