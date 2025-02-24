using C8S.Fulco.Services;
using C8S.UtilityApp.Base;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class TestFulco(
    ILogger<TestFulco> logger,
    TestFulcoOptions options,
    FulcoService fulcoService)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(TestFulco)} : {options.TestAction} ===");
        
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        var startDate = String.IsNullOrEmpty(options.StartDate) ? DateOnly.FromDateTime(DateTime.Today.AddDays(-7)) :
            DateOnly.Parse(options.StartDate);

        var invoiceId = options.InvoiceId ?? 0;

        if (!Enum.TryParse(typeof(FulcoAction), options.TestAction, true, out var testAction))
            throw new Exception($"Could not parse FulcoAction: {options.TestAction}");
        switch (testAction as FulcoAction?)
        {
            case FulcoAction.GetInvoice:
                await RunGetInvoiceTest(invoiceId);
                break;
            case FulcoAction.GetInvoices:
                await RunGetInvoicesTest(startDate, startDate);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testAction), $"Unrecognized FulcoAction: {testAction}");
        }


        logger.LogInformation("{Name}: complete.", nameof(TestFulco));
        return 0;
    }

    private async Task RunGetInvoiceTest(int invoiceId)
    {
        var result = await fulcoService.GetInvoice(invoiceId);
        logger.LogInformation("{InvoiceId}: {@Result}", invoiceId, result);
    }

    private async Task RunGetInvoicesTest(DateOnly startDate, DateOnly endDate)
    {
        var good = await fulcoService.GetInvoices(startDate, endDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", startDate, endDate, good);

        var bad = await fulcoService.GetInvoices(endDate, startDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", endDate, startDate, bad);
    }
}