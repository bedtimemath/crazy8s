using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"add-request", isDefault:false, HelpText = "Add a request and call the endpoint.")]
internal class AddRequestOptions: StandardConsoleOptions
{
}