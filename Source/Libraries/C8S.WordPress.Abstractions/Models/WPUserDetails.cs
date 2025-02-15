namespace C8S.WordPress.Abstractions.Models;

public record WPUserDetails
{
    public int Id { get; init; }
    public string UserName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;

    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public IList<string> Roles { get; init; } = [];
    public IDictionary<string, bool> Capabilities { get; init; } = new Dictionary<string, bool>();
    public IDictionary<string, bool> ExtraCapabilities { get; init; } = new Dictionary<string, bool>();
}