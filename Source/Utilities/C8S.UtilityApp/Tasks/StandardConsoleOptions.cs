using CommandLine;

namespace C8S.UtilityApp.Tasks;

internal abstract class StandardConsoleOptions
{
    private const string DefaultOutputPath = "C:\\"; /* Change to match your desktop */

    [Option(shortName:'o', longName:"output-path", Default = DefaultOutputPath )]
    public string OutputPath { get; set; } = default!;

    [Option(shortName:'p', longName:"platform", Default = "Development" )]
    public string Platform { get; set; } = default!;
}