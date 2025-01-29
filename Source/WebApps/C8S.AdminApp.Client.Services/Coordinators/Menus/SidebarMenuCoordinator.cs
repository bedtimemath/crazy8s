using System.Diagnostics;
using C8S.AdminApp.Client.Services.Pages;
using C8S.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.PubSub;
using SC.Messaging.Abstractions.Interfaces;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public sealed class SidebarMenuCoordinator : IDisposable
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuCoordinator> _logger;
    private readonly IMediator _mediator;
    private readonly IPubSubService _pubSubService;
    #endregion

    #region Public Properties
    public Dictionary<PageGroup, List<PageItem>> PageGroups = new()
    {
        { new PageGroup() { Display = "Requests", Icon = C8SConstants.Icons.Request, Url = "/requests" },  [] },
        { new PageGroup() { Display = "Contacts", Icon = C8SConstants.Icons.Contact, Url = "/contacts" },  [] },
        { new PageGroup() { Display = "Sites", Icon = C8SConstants.Icons.Site, Url = "/sites" },  [] },
        { new PageGroup() { Display = "Organizations", Icon = C8SConstants.Icons.Organization, Url = "/organizations" },  [] },
        { new PageGroup() { Display = "Orders", Icon = C8SConstants.Icons.Order, Url = "/orders" },  [] },
        { new PageGroup() { Display = "Skus", Icon = C8SConstants.Icons.Sku, Url = "/skus" },  [] }
    };
    #endregion

    #region Private Variables
    private readonly Dictionary<string, Action<bool>> _selectedFunctions = new();
    private Func<Task>? _refreshMenuAsync = null;
    #endregion

    #region Constructors / Destructor
    public SidebarMenuCoordinator(
        ILoggerFactory loggerFactory,
        IMediator mediator,
        IPubSubService pubSubService)
    {
        _logger = loggerFactory.CreateLogger<SidebarMenuCoordinator>();
        _mediator = mediator;
        _pubSubService = pubSubService;

        _pubSubService.Subscribe<PageChange>(HandlePageChangeNotification);
    }

    public void Dispose()
    {
        _pubSubService.Unsubscribe<PageChange>(HandlePageChangeNotification);
        _selectedFunctions.Clear();
    }
    #endregion

    #region Public Methods

    public void SetRefreshMenu(Func<Task> refreshMenuAsync) => _refreshMenuAsync = refreshMenuAsync;
    public void ClearRefreshMenu() => _refreshMenuAsync = null;

    public void RegisterComponent(string url, Action<bool> setSelected)
    {
        if (!_selectedFunctions.TryAdd(url, setSelected))
            throw new UnreachableException($"SetSelected function for url already exists: {url}");
    }
    public void UnregisterComponent(string url)
    {
        _selectedFunctions.Remove(url);
    }
    #endregion

    #region Event Handlers
    public async Task HandlePageChangeNotification(PageChange pageChange)
    {
        // add or remove from groups
        if (pageChange.IdValue != null)
        {
            var groupKey = PageGroups.Keys.FirstOrDefault(k => k.Url == "/requests") ??
                           throw new UnreachableException($"Could not find group key: /requests");

            switch (pageChange.Action)
            {
                case PageChangeAction.Open:
                    PageGroups[groupKey].Add(new PageItem()
                    {
                        Display = "NAME HERE",
                        IdValue = pageChange.IdValue,
                        Url = pageChange.NewUrl
                    });
                    break;
                case PageChangeAction.Close:
                    var toRemove = PageGroups[groupKey]
                        .FirstOrDefault(i => i.Url == pageChange.CurrentUrl) ??
                                   throw new UnreachableException($"Could not find item: {pageChange.CurrentUrl}");
                    PageGroups[groupKey].Remove(toRemove);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pageChange));
            }
        }

        // update selected menu items
        foreach (var selectedFunction in _selectedFunctions)
            selectedFunction.Value(selectedFunction.Key == pageChange.NewUrl);

        if (_refreshMenuAsync != null)
            await _refreshMenuAsync.Invoke();
    }
    public async Task HandleSidebarGroupClicked(PageGroup pageGroup)
    {
        await _mediator.Send(new OpenPageCommand() { PageUrl = pageGroup.Url, PageTitle = pageGroup.Display });
    }
    public async Task HandleSidebarItemClicked(PageItem pageItem)
    {
        await _mediator.Send(new OpenPageCommand() { PageUrl = pageItem.Url, PageTitle = pageItem.Display });
    }
    #endregion
}