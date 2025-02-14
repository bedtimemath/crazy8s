namespace C8S.WordPress.Abstractions.Models;

public record WordPressUser
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
}