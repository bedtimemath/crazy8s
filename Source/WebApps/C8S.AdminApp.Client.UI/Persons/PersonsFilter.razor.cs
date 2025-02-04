using C8S.AdminApp.Client.Services.Coordinators.Persons;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Persons;

public sealed partial class PersonsFilter: BaseRazorComponent
{
    [Inject]
    public ILogger<PersonsFilter> Logger { get; set; } = null!;
    
    [Parameter]
    public PersonsListCoordinator Coordinator { get; set; } = null!;
}
