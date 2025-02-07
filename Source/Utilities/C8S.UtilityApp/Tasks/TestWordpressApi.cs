using System.Diagnostics;
using System.Net;
using C8S.UtilityApp.Base;
using Microsoft.Extensions.Logging;
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
        Console.WriteLine($"UserId: {options.UserId}");

        if (options is { TestAction: WordPressApiAction.GetUser or WordPressApiAction.DeleteUser, UserId: null })
            throw new UnreachableException("GetUser action requires a UserId");
        
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
        apiClient.Auth.UseBasicAuth("development@bedtimemath.org", "lX6w eFyu Wypf ZFQf YQEb IwoM");


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
            default:
                throw new ArgumentOutOfRangeException(nameof(options.TestAction), $"Unrecognized WordPressApiAction: {options.TestAction}");
        }

        _logger.LogInformation("{Name}: complete.", nameof(TestWordPressApi));
        return 0;
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