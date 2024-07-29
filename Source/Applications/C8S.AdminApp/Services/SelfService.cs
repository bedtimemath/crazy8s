using C8S.Common;

namespace C8S.AdminApp.Services;

public class SelfService
{
    public string Username => _username ?? SharedConstants.Display.NotSet;
    private string? _username = null;

    public void SetUsername(string username) => _username = username;
}