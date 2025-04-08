using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"fix-crazy8s-data", isDefault:false, HelpText = "Fix crazy8s data already in the database")]
internal class FixC8SDataOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */

    [Option(shortName:'i', longName:"input-path", Default = DefaultInputPath )]
    public string InputPath { get; set; } = default!;
}