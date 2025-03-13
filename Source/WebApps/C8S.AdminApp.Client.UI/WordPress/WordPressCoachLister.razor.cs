using C8S.AdminApp.Client.Services.Coordinators.Temp;
using C8S.WordPress.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.WordPress;

public partial class WordPressCoachLister : BaseCoordinatedComponent<WPCoachListerCoordinator>
{
    [Parameter]
    public WPUserDetails SelectedCoach
    {
        get => Service.SelectedCoaches[0];
        set => Service.SelectCoach(value);
    }

    [Parameter]
    public EventCallback<WPUserDetails> SelectedCoachChanged { get; set; }

}