using C8S.DrawDownApp.Base;
using CommandLine;

namespace C8S.DrawDownApp.Tasks;

[Verb(name:"remove-crazy8s-data", isDefault:false, HelpText = "Remove crazy8s data from the database")]
internal class RemoveC8SDataOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */

    [Option(shortName:'i', longName:"input-path", Default = DefaultInputPath )]
    public string InputPath { get; set; } = null!;
}