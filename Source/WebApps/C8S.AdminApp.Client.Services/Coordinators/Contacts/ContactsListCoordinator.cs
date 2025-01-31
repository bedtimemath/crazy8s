using Microsoft.Extensions.Logging;
using SC.Messaging.Abstractions.Interfaces;
using SC.Messaging.Base;

namespace C8S.AdminApp.Client.Services.Coordinators.Contacts;

public sealed class ContactsListCoordinator(
    ILoggerFactory loggerFactory,
    ICQRSService cqrsService): BaseCQRSCoordinator(cqrsService)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<ContactsListCoordinator> _logger = loggerFactory.CreateLogger<ContactsListCoordinator>();
    #endregion
}
