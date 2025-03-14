using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using C8S.WordPress.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Razor.Base;
#pragma warning disable BL0007

namespace C8S.AdminApp.Client.UI.WordPress;

public partial class WordPressCoachEditor : BaseCoordinatedComponent<WPCoachEditorCoordinator>
{
    [Parameter]
    public WPUserDetails? Coach
    {
        get => Service.Coach; 
        set => Service.SetCoach(value);
    }
}