using C8S.WordPress.Abstractions.Models;
using SC.Common.Client.Base;

namespace C8S.AdminApp.Client.Pages.WordPress;

public sealed partial class WordPressPage : BaseRazorPage
{
    public WPUserDetails? SelectedCoach { get; private set; }
    private void HandleCoachSelected(WPUserDetails coach) => SelectedCoach = coach;
}