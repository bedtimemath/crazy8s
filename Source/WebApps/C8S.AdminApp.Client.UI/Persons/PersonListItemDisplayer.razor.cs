using C8S.Domain.Features.Persons.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Helpers.CQRS.Services;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Persons;

public partial class PersonListItemDisplayer : BaseRazorComponent
{
    [Inject]
    public ICQRSService CQRSService { get; set; } = null!;

    [Parameter]
    public Person Person { get; set; } = null!;
}