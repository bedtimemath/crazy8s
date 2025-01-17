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

        var appointmentId = options.AppointmentId ?? 0;

        if (!Enum.TryParse(typeof(FullSlateAction), options.TestAction, true, out var testAction))
            throw new Exception($"Could not parse FullSlateAction: {options.TestAction}");
        switch (testAction as FullSlateAction?)
        {
            case FullSlateAction.AddAppointment:
                await RunAddAppointmentTest(startDate, endDate);
                break;
            case FullSlateAction.AddClient:
                await RunAddClientTest();
                break;
            case FullSlateAction.GetAppointment:
                await RunGetAppointmentTest(appointmentId);
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
        var getOpeningsResponse = await fullSlateService.GetOpeningsList(startDate, endDate) ??
                                  throw new Exception($"Could not get openings from {startDate:d} to {endDate:d}");

        // Offerings
        var offerings = getOpeningsResponse.Data?.Offerings ??
                        throw new Exception($"No offerings from {startDate:d} to {endDate:d}");
        foreach (var offering in offerings)
            logger.LogInformation("\t{@Offering}", offering);

        // Openings
        var openings = getOpeningsResponse.Data?.Openings ??
                       throw new Exception($"No openings from {startDate:d} to {endDate:d}");
        logger.LogInformation("{FromDate:d}-{ToDate:d}: Found {@Count} openings", startDate, endDate, openings.Count);
        var lastOpening = openings[^1];
        logger.LogInformation("Last opening: {@Last}", lastOpening);

        // Add Appointment
        var appointmentCreation = new FullSlateAppointmentCreation()
        {
            At = lastOpening,
            Services = [ FullSlateConstants.Offerings.CoachCall ],
            Client = new FullSlateAppointmentCreationClient()
            {
                FirstName = "Testly",
                LastName = "Testerson",
                Email = "dsteen+testy@gmail.com",
                PhoneNumber = new FullSlatePhoneNumber() { Number = "9998881111" }
            },
            UserTypeString = FullSlateConstants.UserTypes.Client
        };
        logger.LogInformation("Creation: {@Creation}", appointmentCreation);

        var successResponse = await fullSlateService.AddAppointment(appointmentCreation);
        logger.LogInformation("Success Response: {@Response}", successResponse);

        var failureResponse = await fullSlateService.AddAppointment(appointmentCreation);
        logger.LogInformation("Failure Response: {@Response}", failureResponse);
    }

    private async Task RunAddClientTest()
    {
        // Add Client
        var clientCreation = new FullSlateClientCreation()
        {
            FirstName = "Testly",
            LastName = "Testers",
            PhoneNumbers = [ new FullSlatePhoneNumber() { Number = "9708218128" } ],
            Emails = [ "dsteen+testly@gmail.com"]
        };
        logger.LogInformation("Creation: {@Creation}", clientCreation);

        var addClientsResponse = await fullSlateService.AddClient(clientCreation);
        logger.LogInformation("Response: {@Response}", addClientsResponse);
    }

    private async Task RunGetAppointmentTest(int appointmentId)
    {
        var result = await fullSlateService.GetAppointment(appointmentId);
        logger.LogInformation("{AppointmentId}: {@Result}", appointmentId, result);
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