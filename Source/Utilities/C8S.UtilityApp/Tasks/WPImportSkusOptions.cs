using C8S.UtilityApp.Base;
using CommandLine;

namespace C8S.UtilityApp.Tasks;
[Verb(name:"wp-import-skus", isDefault:false, HelpText = "Import SKUs to WordPress.")]
internal class WPImportSkusOptions: StandardConsoleOptions
{
    //private const string DefaultInputPath = "C:\\Git\\Crazy8s\\Data"; /* Change to match your desktop */
    private const string DefaultSite = "https://coaches.crazy8sclub.org/wp-json/";

    [Option(shortName:'s', longName:"site", Default = DefaultSite )]
    public string Site { get; set; } = DefaultSite;
}