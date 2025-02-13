using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;

internal enum WordPressApiAction
{
    AllActions,
    AddUser,
    GetUser,
    GetAllUsers,
    DeleteUser,
    GetAllSkus,
    GetAllActivities,
    UpdateActivity
}

[Verb(name:"test-wordpress-api", isDefault:false, HelpText = "Test the WordPress API.")]
internal class TestWordPressApiOptions: StandardConsoleOptions
{
    //private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */

    private const string DefaultSite = "https://coaches.crazy8sclub.org/wp-json/";
    private const WordPressApiAction DefaultTestAction = WordPressApiAction.AllActions;

    [Option(shortName:'s', longName:"site", Default = DefaultSite )]
    public string Site { get; set; } = DefaultSite;

    [Option(shortName: 'a', longName: "test-action", Default = DefaultTestAction)]
    public WordPressApiAction TestAction { get; set; } = DefaultTestAction;

    [Option(shortName: 'u', longName: "user-id", Default = null)]
    public int? UserId { get; set; } = null;

    [Option(shortName: 'i', longName: "activity-id", Default = null)]
    public int? ActivityId { get; set; } = null;

}