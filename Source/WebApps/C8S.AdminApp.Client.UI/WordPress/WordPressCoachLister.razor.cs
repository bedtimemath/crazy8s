using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using C8S.WordPress.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.WordPress;

public partial class WordPressCoachLister : BaseCoordinatedComponent<WPCoachListerCoordinator>
{
    [Parameter]
    public EventCallback<WPUserDetails> CoachSelected { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Service.CoachSelected = CoachSelected;
    }
}