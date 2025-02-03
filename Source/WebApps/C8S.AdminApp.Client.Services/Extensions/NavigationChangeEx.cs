using System.Diagnostics;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Models;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationChangeEx
{
    public static MenuItem ToMenuItem(this NavigationChange navigationChange,
        Func<string> createDisplay, Func<string> createUrl) =>
        new()
        {
            Entity = navigationChange.Entity ??
                     throw new UnreachableException("Missing Entity"),
            Display = createDisplay(),
            Url = createUrl(),
            IdValue = navigationChange.IdValue ??
                      throw new UnreachableException("Missing IdValue")
        };
}