namespace C8S.UtilityApp.Tasks;

// see: https://jasonterando.medium.com/net-core-console-applications-with-dependency-injection-234eac5a4040
public interface IActionLauncher
{
    Task<int> Launch();
}