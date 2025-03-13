using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using C8S.WordPress.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using SC.Common.Client.Base;

namespace C8S.AdminApp.Client.Pages.WordPress;

public sealed partial class WordPressPage :
    BaseCoordinatedPage<WordPressCoordinator>
{
    public WPUserDetails? SelectedCoach { get; set; } = null!;
}