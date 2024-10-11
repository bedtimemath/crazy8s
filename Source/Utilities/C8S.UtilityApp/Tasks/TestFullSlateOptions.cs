using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

internal enum FullSlateAction
{
    AddAppointment,
    AddClient,
    GetAppointments,
    GetOpeningsList
}

[Verb(name:"test-fullslate", isDefault:false, HelpText = "Test the FullSlate API.")]
internal class TestFullSlateOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
    
    private const FullSlateAction DefaultTestAction = FullSlateAction.GetAppointments;

    [Option(shortName:'a', longName:"test-action", Default = DefaultTestAction )]
    public string TestAction { get; set; } = default!;

    [Option(shortName:'e', longName:"end-date", Default = null)]
    public string? EndDate { get; set; } = default!;
}