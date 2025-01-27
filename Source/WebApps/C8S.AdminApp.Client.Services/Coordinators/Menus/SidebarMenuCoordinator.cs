using C8S.AdminApp.Client.Services.Pages;
using C8S.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using SC.Common.PubSub;

namespace C8S.AdminApp.Client.Services.Coordinators.Menus;

public class SidebarMenuCoordinator(
    ILoggerFactory loggerFactory,
    IMediator mediator)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<SidebarMenuCoordinator> _logger = loggerFactory.CreateLogger<SidebarMenuCoordinator>();
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

    #region Event Handlers
    public async Task HandlePageChangeNotification(PageChange pageChange)
    {
        _logger.LogDebug("PageChange={@PageChange}", pageChange);
    }
    public async Task HandleSidebarGroupClicked(PageGroup pageGroup)
    {
        _logger.LogDebug("PageGroup={@PageGroup}", pageGroup);
        await mediator.Send(new OpenPageCommand() { PageUrl = pageGroup.Url, PageTitle = pageGroup.Display });
    }
    #endregion

}