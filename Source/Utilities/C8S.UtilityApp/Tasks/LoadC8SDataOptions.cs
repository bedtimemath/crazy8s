using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"load-crazy8s-data", isDefault:false, HelpText = "Load crazy8s data into the database")]
internal class LoadC8SDataOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */

    [Option(shortName:'i', longName:"input-path", Default = DefaultInputPath )]
    public string InputPath { get; set; } = default!;
}