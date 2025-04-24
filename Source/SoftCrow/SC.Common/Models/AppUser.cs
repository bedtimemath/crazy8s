namespace SC.Common.Models;

public class AppUser
{
    public Guid? SessionId { get; set; }
    public string? AuthIdentifier { get; set; }
    public string? DisplayName { get; set; }
    public string? EmailAddress { get; set; }
    public string? AvatarUrl { get; set; }
    public List<string>? Roles { get; set; }

}