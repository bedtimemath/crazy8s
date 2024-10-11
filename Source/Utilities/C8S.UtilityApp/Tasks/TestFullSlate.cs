using C8S.FullSlate.Abstractions;
using C8S.FullSlate.Abstractions.Models;
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
            case FullSlateAction.AddAppointment:
                await RunAddAppointmentTest(startDate, endDate);
                break;
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

    private async Task RunAddAppointmentTest(DateOnly startDate, DateOnly endDate)
    {
        var fullSlateResponse = await fullSlateService.GetOpeningsList(startDate, endDate) ??
                           throw new Exception($"Could not get openings from {startDate:d} to {endDate:d}");

        // Offerings
        var offerings = fullSlateResponse.Data?.Offerings ??
                           throw new Exception($"No offerings from {startDate:d} to {endDate:d}");
        foreach (var offering in offerings)
            logger.LogInformation("\t{@Offering}", offering);

        // Openings
        var openings = fullSlateResponse.Data?.Openings ??
                       throw new Exception($"No openings from {startDate:d} to {endDate:d}");
        logger.LogInformation("{FromDate:d}-{ToDate:d}: Found {@Count} openings", startDate, endDate, openings.Count);
        var lastOpening = openings[^1];
        logger.LogInformation("Last opening: {@Last}", lastOpening);

        // Add Appointment
        var appointmentCreation = new FullSlateAppointmentCreation()
        {
            AtDateTime = lastOpening,
            Services = [ FullSlateConstants.Offerings.CoachCall ]
        };
        logger.LogInformation("Creation: {@Creation}", appointmentCreation);
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