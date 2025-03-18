
namespace C8S.WordPress.Abstractions.MagicLogin;

public class WPMagicLoginRequest
{
    public string? user { get; set; }
    public bool send { get; set; } = false;
    public string? redirect_to { get; set; }
}