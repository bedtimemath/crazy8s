using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;


[Verb(name:"test-interceptors", isDefault:false, HelpText = "Test the EF Core Interceptors.")]
internal class TestInterceptorsOptions: StandardConsoleOptions
{
}