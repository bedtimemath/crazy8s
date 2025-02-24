using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

internal enum FulcoAction
{
    GetInvoice,
    GetInvoices
}

[Verb(name:"test-fulco", isDefault:false, HelpText = "Test the Fulco API.")]
internal class TestFulcoOptions: StandardConsoleOptions
{
    private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
    
    private const FulcoAction DefaultTestAction = FulcoAction.GetInvoices;

    [Option(shortName:'a', longName:"test-action", Default = DefaultTestAction )]
    public string TestAction { get; set; } = null!;

    [Option(shortName:'s', longName:"start-date", Default = null)]
    public string? StartDate { get; set; } = null;

    [Option(shortName:'i', longName:"invoice-id", Default = null )]
    public int? InvoiceId { get; set; } = null;


}