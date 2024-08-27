using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

[Verb(name:"load-sample-data", isDefault:false, HelpText = "Load sample data into the database")]
internal class LoadSampleDataOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
    private const string DefaultCoachFile = "coaches.csv";

    [Option(shortName:'i', longName:"input-path", Default = DefaultInputPath )]
    public string InputPath { get; set; } = default!;

    [Option(shortName:'c', longName:"coach-file", Default = DefaultCoachFile )]
    public string CoachFile { get; set; } = default!;
}