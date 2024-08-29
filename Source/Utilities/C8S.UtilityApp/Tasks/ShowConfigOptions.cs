using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"show-config", isDefault:false, HelpText = "Show configuration options.")]
internal class ShowConfigOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
}