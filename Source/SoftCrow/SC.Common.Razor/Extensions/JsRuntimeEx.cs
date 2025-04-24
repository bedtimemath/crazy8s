using Microsoft.JSInterop;

namespace SC.Common.Razor.Extensions;

public static class JsRuntimeEx
{
    // requires the softcrow-common javascript file
    public static async Task ScrollToTop(this IJSRuntime jsRuntime, string containerId)
    {
        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", 
                "./_content/SC.Common.Razor/js/softcrow-common.js");
        await module.InvokeVoidAsync("scrollToTop", containerId);
    }

    // requires the softcrow-common javascript file
    public static async Task<string> GetTextBoxValue(this IJSRuntime jsRuntime, string textBoxId)
    {
        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", 
                "./_content/SC.Common.Razor/js/softcrow-common.js");
        return await module.InvokeAsync<string>("getTextBoxValue", textBoxId);
    }

    // requires the softcrow-common javascript file
    public static async Task SetFocus(this IJSRuntime jsRuntime, string componentId)
    {
        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", 
                "./_content/SC.Common.Razor/js/softcrow-common.js"); 
        await module.InvokeVoidAsync("setFocus", componentId);
    }

    // requires the softcrow-common javascript file
    public static async Task ShowAlert(this IJSRuntime jsRuntime, string message)
    {
        await using var module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", 
                "./_content/SC.Common.Razor/js/softcrow-common.js"); 
        await module.InvokeVoidAsync("showAlert", message);
    }
}