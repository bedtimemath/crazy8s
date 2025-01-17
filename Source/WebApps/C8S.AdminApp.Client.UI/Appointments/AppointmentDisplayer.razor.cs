﻿using Blazr.RenderState;
using C8S.AdminApp.Client.Services.Coordinators.Appointments;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SC.Common.Razor.Base;

namespace C8S.AdminApp.Client.UI.Appointments;

public partial class AppointmentDisplayer : 
    BaseOwningComponent<AppointmentDisplayerCoordinator>
{
    #region Injected Properties
    [Inject]
    public ILogger<AppointmentDisplayer> Logger { get; set; } = null!;

    [Inject]
    public IBlazrRenderStateService RenderStateService { get; set; } = null!;
    #endregion

    #region Component Parameters
    [Parameter]
    public bool RemoveOuterDiv { get; set; } = false;
    
    [Parameter]
    public int RequestId { get; set; } 
    
    [Parameter]
    public int? AppointmentId { get; set; } 

    [Parameter]
    public DateTimeOffset? AppointmentStartsOn { get; set; } 
    #endregion

    #region Component LifeCycle
    protected override void OnInitialized()
    {
        Service.DetailsUpdated += HandleDetailsUpdated;
        base.OnInitialized();
    }

    public void Dispose()
    {
        Service.DetailsUpdated -= HandleDetailsUpdated;
    }

    protected override void OnParametersSet()
    {
        Logger.LogDebug("OnParametersSet");
        base.OnParametersSet();
        
        if (!RenderStateService.IsPreRender)
            Service.SetDetailsId(RequestId, AppointmentId, AppointmentStartsOn);
    }
    #endregion

    #region Event Handlers
    private void HandleDetailsUpdated(object? sender, EventArgs e) => 
        StateHasChanged();
    #endregion
}