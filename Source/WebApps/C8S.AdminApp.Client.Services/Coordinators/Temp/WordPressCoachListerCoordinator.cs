using C8S.WordPress.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Radzen;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Temp;

public sealed class WordPressCoachListerCoordinator(
    ILoggerFactory loggerFactory,
    IPubSubService pubSubService,
    ICQRSService cqrsService) : BaseCoordinator(loggerFactory, pubSubService, cqrsService)
{
    private readonly ILogger<WordPressCoachListerCoordinator> _logger =
        loggerFactory.CreateLogger<WordPressCoachListerCoordinator>();

    public IEnumerable<WordPressUserDetails> Coaches { get; set; } = new List<WordPressUserDetails>();
    public IList<WordPressUserDetails> SelectedCoaches { get; set; } = [];
    public int TotalCount { get; set; }

    public bool IsLoading { get; set; } = false;

    public override void SetUp()
    {
        base.SetUp();
        LoadCoaches(new LoadDataArgs() { Skip = 0, Top = 5 });
    }

    public void LoadCoaches(LoadDataArgs args) =>
        Task.Run(async () => await LoadCoachesAsync(args));

    private async Task LoadCoachesAsync(LoadDataArgs args)
    {
        if (!String.IsNullOrWhiteSpace(args.Filter) && args.Filter.Length < 3) return;
        try
        {
            IsLoading = true;
            await PerformComponentRefresh();

#if false
            var personsListResult = await GetQueryResults<PersonsListQuery, DomainResponse<PersonsListResults>>(
        new PersonsListQuery()
        {
            StartIndex = args.Skip ?? 0,
            Count = args.Top ?? 0,
            Query = String.IsNullOrWhiteSpace(args.Filter) ? null : args.Filter,
            SortDescription = "Email ASC"
        }) ?? throw new UnreachableException("GetQueryResults returned null");

            if (!personsListResult.Success || personsListResult.Result == null)
                throw personsListResult.Exception?.ToException() ??
                      new Exception("Error occurred loading persons");

            Persons = personsListResult.Result.Items;
            TotalPersons = personsListResult.Result.Total;

            _logger.LogDebug("Skip={Skip}, Top={Top}, Total={Total}",
                args.Skip, args.Top, TotalPersons); 
#endif

            IsLoading = false;
            await PerformComponentRefresh();
        }
        catch (Exception ex)
        {
            PubSubService.PublishException(ex);
        }
    }
}