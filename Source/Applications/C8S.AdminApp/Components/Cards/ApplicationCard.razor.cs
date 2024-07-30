using C8S.Common.Razor.Base;
using C8S.Database.Abstractions.DTOs;
using Microsoft.AspNetCore.Components;

namespace C8S.AdminApp.Components.Cards;

public partial class ApplicationCard: BaseRazorComponent
{
    [Parameter]
    public ApplicationDTO Application { get; set; } = default!;
}