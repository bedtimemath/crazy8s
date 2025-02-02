using System.Diagnostics;
using C8S.AdminApp.Client.Services.Menu.Models;
using C8S.AdminApp.Client.Services.Navigation.Models;

namespace C8S.AdminApp.Client.Services.Extensions;

public static class NavigationChangeEx
{
    public static MenuItem ToMenuItem(this NavigationChange navigationChange,
        Func<string> createDisplay, Func<string> createUrl) =>
        new MenuItem()
        {
            Entity = navigationChange.Entity,
            Display = createDisplay(),
            Url = createUrl(),
            IdValue = navigationChange.IdValue ??
                      throw new UnreachableException("Missing IdValue"),
            JsonDetails = navigationChange.JsonDetails ??
                          throw new UnreachableException("Missing JsonDetails")
        };
}