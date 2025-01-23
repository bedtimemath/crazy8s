using Radzen;

namespace C8S.AdminApp.Client.UI.Common;

public static class RadzenDialogOptions
{
    public static DialogOptions Narrow = new()
    {
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Width = "450px",
    };
    public static DialogOptions Standard = new()
    {
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Width = "600px",
    };
    public static DialogOptions Wide = new()
    {
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Width = "800px",
    };
    public static DialogOptions ExtraWide = new()
    {
        CloseDialogOnEsc = true,
        CloseDialogOnOverlayClick = true,
        Width = "1080px",
    };
}