using C8S.FullSlate.Services;
using C8S.UtilityApp.Base;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class TestFullSlate(
    ILogger<TestFullSlate> logger,
    TestFullSlateOptions options,
    FullSlateService fullSlateService)
    : IActionLauncher
{
    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(TestFullSlate)} : {options.TestAction} ===");
        
        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = String.IsNullOrEmpty(options.EndDate) ? DateOnly.FromDateTime(DateTime.Today.AddDays(7)) :
            DateOnly.Parse(options.EndDate);

        if (!Enum.TryParse(typeof(FullSlateAction), options.TestAction, true, out var testAction))
            throw new Exception($"Could not parse FullSlateAction: {options.TestAction}");
        switch (testAction as FullSlateAction?)
        {
            case FullSlateAction.GetAppointments:
                await RunGetAppointmentsTest(startDate, endDate);
                break;
            case FullSlateAction.GetOpeningsList:
                await RunGetOpeningsListTest(startDate, endDate);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(testAction), $"Unrecognized FullSlateAction: {testAction}");
        }


        logger.LogInformation("{Name}: complete.", nameof(TestFullSlate));
        return 0;
    }

    private async Task RunGetAppointmentsTest(DateOnly startDate, DateOnly endDate)
    {
        var good = await fullSlateService.GetAppointments(startDate, endDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", startDate, endDate, good);

        var bad = await fullSlateService.GetAppointments(endDate, startDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", endDate, startDate, bad);
    }

    private async Task RunGetOpeningsListTest(DateOnly startDate, DateOnly endDate)
    {
        var good = await fullSlateService.GetOpeningsList(startDate, endDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", startDate, endDate, good);

        var bad = await fullSlateService.GetOpeningsList(endDate, startDate);
        logger.LogInformation("{FromDate:d}-{ToDate:d}: {@Result}", endDate, startDate, bad);
    }
}