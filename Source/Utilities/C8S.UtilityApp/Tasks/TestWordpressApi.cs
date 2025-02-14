using System.Diagnostics;
using System.Net;
using C8S.UtilityApp.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SC.Common.Extensions;
using WordPressPCL;
using WordPressPCL.Models;

namespace C8S.UtilityApp.Tasks;

internal class TestWordPressApi(
    ILoggerFactory loggerFactory,
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

        var handler = new HttpClientHandler()
        {
            //Proxy = new WebProxy("http://localhost:8866", false)
        };
        _httpClient = new HttpClient(handler) { BaseAddress = new Uri(options.Site) };
        var apiClient = new WordPressClient(_httpClient);
        apiClient.Auth.UseBasicAuth("development@bedtimemath.org", "rkz0 2cff PoAk WsBu Eyu9 d84W");
        

        switch (options.TestAction)
        {
            case WordPressApiAction.AllActions:
                Console.WriteLine("== GET ALL USERS ==");
                await RunGetAllUsersTest(apiClient);
                Console.WriteLine("== ADD USER ==");
                var userId = await RunAddUserTest(apiClient);
                Console.WriteLine("== GET USER ==");
                await RunGetUserTest(apiClient, userId);
                Console.WriteLine("== DELETE USER ==");
                await RunDeleteUserTest(apiClient, userId);
                break;
            case WordPressApiAction.AddUser:
                await RunAddUserTest(apiClient);
                break;
            case WordPressApiAction.GetUser:
                await RunGetUserTest(apiClient, options.UserId!.Value);
                break;
            case WordPressApiAction.DeleteUser:
                await RunDeleteUserTest(apiClient, options.UserId!.Value);
                break;
            case WordPressApiAction.GetAllUsers:
                await RunGetAllUsersTest(apiClient);
                break;
            case WordPressApiAction.GetAllSkus:
                await RunGetAllSkusTest(apiClient);
                break;
            case WordPressApiAction.GetAllActivities:
                await RunGetAllActivitiesTest(apiClient);
                break;
            case WordPressApiAction.UpdateActivity:
                Console.WriteLine("== UPDATE ACTIVITY ==");
                await RunUpdateActivityTest(apiClient, options.ActivityId!.Value);
                Console.WriteLine("== GET ALL ACTIVITIES ==");
                await RunGetAllActivitiesTest(apiClient);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(options.TestAction), $"Unrecognized WordPressApiAction: {options.TestAction}");
        }

        _logger.LogInformation("{Name}: complete.", nameof(TestWordPressApi));
        return 0;
    }

    private async Task RunGetAllSkusTest(WordPressClient apiClient)
    {
        try
        {
            var skus = await apiClient.CustomRequest.GetAsync<IEnumerable<Sku>>("/wp-json/wp/v2/sku");
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

    private async Task RunGetAllActivitiesTest(WordPressClient apiClient)
    {
        try
        {
            var activities = await apiClient.CustomRequest.GetAsync<IEnumerable<Activity>>("/wp-json/wp/v2/activity");
            foreach (var activity in activities)
            {
                _logger.LogDebug("{@Activity}", activity);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunUpdateActivityTest(WordPressClient apiClient, int activityId)
    {
        try
        {
            var activities = await apiClient.CustomRequest.GetAsync<IEnumerable<Activity>>($"/wp-json/wp/v2/activity");
            var activity = activities.FirstOrDefault(a => a.ActivityId == activityId) ??
                           throw new UnreachableException($"Could not find activity #{activityId}");

            activity.Properties ??= new ActivityProperties();
            activity.Properties.Materials ??= [];
            activity.Properties.Materials.Add(new ActivityMaterial() { Quantity = 3, Material = "Triangles" });
            activity.Properties.Materials.Add(new ActivityMaterial() { Quantity = 6, Material = "Hexagons" });

            var updated = await apiClient.CustomRequest.UpdateAsync<Activity, Activity>($"/wp-json/wp/v2/activity/{activityId}", activity);

            _logger.LogDebug("{@Activity}", updated);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }

    private async Task RunGetAllUsersTest(WordPressClient apiClient)
    {
        try
        {
            var users = await apiClient.Users.GetAllAsync(useAuth:true);
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

    private async Task<int> RunAddUserTest(WordPressClient apiClient)
    {
        try
        {
            var extraBit = String.Empty.AppendRandomAlphaOnly(5);
            var username = $"test_{extraBit}";
            var email = $"dsteen+test{extraBit}@gmail.com";
            var user = await apiClient.Users.CreateAsync(new User(username, email, "Test123!")
            {
                Roles = ["subscriber","coach"]
            });
            _logger.LogDebug("Created: {@User}", user);
            return user.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
            return 0;
        }
    }
    
    private async Task RunGetUserTest(WordPressClient apiClient, int id)
    {
        try
        {
            var user = await apiClient.Users.GetByIDAsync(id, useAuth:true);
            _logger.LogDebug("Got #{Id}:{@User}", id, user);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }
    
    private async Task RunDeleteUserTest(WordPressClient apiClient, int id) 
    {
        try
        {
            var user = await apiClient.Users.Delete(id,1);
            _logger.LogDebug("Deleted #{Id}:{@User}", id, user);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Could not run test");
        }
    }
}

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

public class Sku
{
    [JsonProperty(PropertyName = "id")]
    public int SkuId { get; set; }
    [JsonProperty(PropertyName = "slug")]
    public string? Slug { get; set; }
    [JsonProperty(PropertyName = "status")]
    public string? Status { get; set; }
    [JsonProperty(PropertyName = "link")]
    public string? Link { get; set; }
    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }
    [JsonProperty(PropertyName = "title")]
    public SkuTitle? Title { get; set; }
    [JsonProperty(PropertyName = "acf")]
    public SkuProperties? Properties { get; set; }
}

public class SkuProperties
{
    [JsonProperty(PropertyName = "sku_name")]
    public string? SkuName { get; set; }
    [JsonProperty(PropertyName = "sku_identifier")]
    public string? SkuIdentifier { get; set; }
    [JsonProperty(PropertyName = "season")]
    public string? Season { get; set; }
    [JsonProperty(PropertyName = "age_range")]
    public string? AgeRange { get; set; }
}

public class SkuTitle
{
    [JsonProperty(PropertyName = "rendered")]
    public string? Rendered { get; set; }
}