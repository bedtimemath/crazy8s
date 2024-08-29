using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"process-applications", isDefault:false, HelpText = "Process Crazy 8s applications.")]
internal class ProcessApplicationsOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
}