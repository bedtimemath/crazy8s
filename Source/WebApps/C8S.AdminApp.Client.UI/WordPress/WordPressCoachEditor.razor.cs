using C8S.AdminApp.Client.Services.Coordinators.WordPress;
using C8S.WordPress.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using SC.Common.Razor.Base;
#pragma warning disable BL0007

namespace C8S.AdminApp.Client.UI.WordPress;

public partial class WordPressCoachEditor : BaseCoordinatedComponent<WPCoachEditorCoordinator>
{
    [Inject]
    public DialogService DialogService { get; set; } = null!;

    [Parameter]
    public WPUserDetails Coach
    {
        get => Service.Coach; 
        set => Service.SetCoach(value);
    }

    private bool _isDeleting;

    private async Task ConfirmDelete() 
    {
        var result = await DialogService
            .Confirm($"Are you sure you want to remove '{Coach.Name}' from WordPress?", "Delete Coach");
        if (!(result ?? false)) return;

        _isDeleting = true;
        await InvokeAsync(StateHasChanged);

        try
        {
            await Service.HandleDeleteClicked();
        }
        finally
        {
            _isDeleting = false;
            await InvokeAsync(StateHasChanged);
        }
    }
}