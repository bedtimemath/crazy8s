using Microsoft.Extensions.Logging;

namespace C8S.AdminApp.Client.Services.Coordinators.Contacts;

public sealed class ContactsListCoordinator(
    ILoggerFactory loggerFactory)
{
    #region ReadOnly Constructor Variables
    private readonly ILogger<ContactsListCoordinator> _logger = loggerFactory.CreateLogger<ContactsListCoordinator>();
    #endregion
}
