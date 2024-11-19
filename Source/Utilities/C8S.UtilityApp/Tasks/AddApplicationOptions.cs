using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"add-application", isDefault:false, HelpText = "Add an application and call the endpoint.")]
internal class AddApplicationOptions: StandardConsoleOptions
{
}