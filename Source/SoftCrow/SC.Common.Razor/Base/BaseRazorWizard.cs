using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace SC.Common.Razor.Base;

public abstract class BaseRazorWizard<TModel>: BaseRazorComponent
    where TModel: class, new()
{
    #region Injected Variables
    //[Inject]
    //protected ILogger<BaseRazorWizard<TModel>> BaseLogger { get; set; } = null!;
    [Inject]
    protected DialogService DialogService { get; set; } = null!;
    [Inject]
    protected NotificationService NotificationService { get; set; } = null!;
    #endregion

    #region Wizard Components
    protected RadzenTemplateForm<TModel> ModelForm = null!;
    protected RadzenSteps WizardSteps = null!;
    #endregion

    #region Protected Properties
    protected int StepIndex
    {
        get => _stepIndex;
        set
        {
            if (_stepIndex == value) return;
            _stepIndex = value;

            UpdatePreviousNext();
        }
    }
    private int _stepIndex = 0;
    #endregion

    #region Protected Variables
    protected bool OnFirstStep = true;
    protected bool OnLastStep = false;
    protected bool AllowContinue = false;

    // since we can't get this until after render, we need to give
    //  a legit value for inner properties; so we start with "new" not "default!"
    protected readonly TModel Model = new();
    #endregion

    #region Wizard LifeCycle
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (!firstRender) return;
        await HandleModelChanged(Model).ConfigureAwait(false);
    }
    #endregion

    #region Event Handlers (Model)
    protected virtual Task HandleModelChanged(TModel _)
    {
        UpdatePreviousNext();
        return Task.CompletedTask;
    }
    #endregion

    #region Event Handlers (Wizard)
    protected virtual async Task HandlePreviousButtonClicked()
    {
        try
        {
            if (StepIndex <= 0) return;
            StepIndex--;
        }
        catch (Exception exception)
        {
            // todo
            //await RaiseExceptionAsync(exception).ConfigureAwait(false);
        }
    }
    protected virtual async Task HandleNextButtonClicked()
    {
        try
        {
            if (StepIndex >= (WizardSteps.StepsCollection.Count - 1)) return;
            StepIndex++;
        }
        catch (Exception exception)
        {
            // todo
            //await RaiseExceptionAsync(exception).ConfigureAwait(false);
        }
    }
    #endregion

    #region Protected Methods
    protected virtual void UpdatePreviousNext()
    {
        var steps = WizardSteps?.StepsCollection;

        OnFirstStep = _stepIndex == 0;
        OnLastStep = _stepIndex == ((WizardSteps?.StepsCollection?.Count ?? 1) - 1);
        AllowContinue = true;
    }
    #endregion
}