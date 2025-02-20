using System.Diagnostics;
using C8S.UtilityApp.Base;
using C8S.WordPress.Services;
using Microsoft.Extensions.Logging;

namespace C8S.UtilityApp.Tasks;

internal class TestWordPressApi(
    ILoggerFactory loggerFactory,
    WordPressService wordPressService,
    TestWordPressApiOptions options)
    : IActionLauncher
{
    private readonly ILogger<TestWordPressApi> _logger = loggerFactory.CreateLogger<TestWordPressApi>();
    private HttpClient _httpClient = null!;

    public async Task<int> Launch()
    {
        Console.WriteLine($"=== {nameof(TestWordPressApi)} : {options.TestAction} ===");

        Console.WriteLine($"Site: {options.Site}");
        Console.WriteLine($"Action: {options.TestAction}");
        Console.WriteLine($"ActivityId: {options.ActivityId}");
        Console.WriteLine($"UserId: {options.UserId}");

        if (options is { TestAction: WordPressApiAction.GetUser or WordPressApiAction.DeleteUser, UserId: null })
            throw new UnreachableException("GetUser / DeleteUser action requires a UserId");
        if (options is { TestAction: WordPressApiAction.UpdateActivity, ActivityId: null })
            throw new UnreachableException("UpdateActivity action requires a ActivityId");

        Console.WriteLine("Continue? Y/N");
        var checkContinue = Console.ReadKey();

        var firstChar = Char.ToLower(checkContinue.KeyChar);
        if (firstChar != 'y') return 0;

        Console.WriteLine();

        switch (options.TestAction)
        {
            case WordPressApiAction.AllActions:
                Console.WriteLine("== GET ALL USERS ==");
                await RunGetAllUsersTest();
                Console.WriteLine("== ADD USER ==");
                var userId = await RunAddUserTest();
                Console.WriteLine("== GET USER ==");
                await RunGetUserTest(userId);
                Console.WriteLine("== DELETE USER ==");
                await RunDeleteUserTest(userId);
                break;
            case WordPressApiAction.AddUser:
                await RunAddUserTest();
                break;
            case WordPressApiAction.GetUser:
                await RunGetUserTest(options.UserId!.Value);
                break;
            case WordPressApiAction.DeleteUser:
                await RunDeleteUserTest(options.UserId!.Value);
                break;
            case WordPressApiAction.GetAllUsers:
                await RunGetAllUsersTest();
                break;
            case WordPressApiAction.GetAllSkus:
                await RunGetAllSkusTest();
                break;
            case WordPressApiAction.GetAllActivities:
                await RunGetAllActivitiesTest();
                break;
            case WordPressApiAction.UpdateActivity:
                Console.WriteLine("== UPDATE ACTIVITY ==");
                await RunUpdateActivityTest(options.ActivityId!.Value);
                Console.WriteLine("== GET ALL ACTIVITIES ==");
                await RunGetAllActivitiesTest();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(options.TestAction), $"Unrecognized WordPressApiAction: {options.TestAction}");
        }

        _logger.LogInformation("{Name}: complete.", nameof(TestWordPressApi));
        return 0;
    }

    private async Task RunGetAllSkusTest()
    {
        try
        {
            var skusResult = await wordPressService.GetWordPressSkus();
            var skus = skusResult.Result ?? throw new Exception("No Skus returned.");
            foreach (var sku in skus)
            {
                _logger.LogDebug("{@Sku}", sku);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunGetAllActivitiesTest()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunUpdateActivityTest( int activityId)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunGetAllUsersTest()
    {
        try
        {
            var usersResult = await wordPressService.GetWordPressUsers();
            var users = usersResult.Result ?? throw new Exception("No Users returned.");
            foreach (var user in users)
            {
                _logger.LogDebug("{@User}", user);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task<int> RunAddUserTest()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
            return 0;
        }
    }

    private async Task RunGetUserTest( int id)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunDeleteUserTest( int id)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }
}

#if false
public class Activity
{
    [JsonProperty(PropertyName = "id")]
    public int ActivityId { get; set; }
    [JsonProperty(PropertyName = "slug")]
    public string? Slug { get; set; }
    [JsonProperty(PropertyName = "status")]
    public string? Status { get; set; }
    [JsonProperty(PropertyName = "link")]
    public string? Link { get; set; }
    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }
    [JsonProperty(PropertyName = "acf")]
    public ActivityProperties? Properties { get; set; }
}

public class ActivityProperties
{
    [JsonProperty(PropertyName = "sku")]
    public List<int>? Sku { get; set; }
    [JsonProperty(PropertyName = "materials")]
    public List<ActivityMaterial>? Materials { get; set; }
}

public class ActivityMaterial
{
    [JsonProperty(PropertyName = "quantity")]
    public int? Quantity { get; set; }
    [JsonProperty(PropertyName = "material")]
    public string? Material { get; set; }
} 
#endif