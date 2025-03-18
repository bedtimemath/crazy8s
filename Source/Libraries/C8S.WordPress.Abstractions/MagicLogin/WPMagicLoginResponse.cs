namespace C8S.WordPress.Abstractions.MagicLogin;

public class WPMagicLoginResponse
{
    public string? link { get; set; }
    public bool? mail_sent { get; set; }
    public string? code { get; set; }
    public string? message { get; set; }
}