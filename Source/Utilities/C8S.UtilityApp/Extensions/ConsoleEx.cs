namespace C8S.UtilityApp.Extensions;

public static class ConsoleEx
{
    public static void StartProgress(string? message = null)
    {
        Console.Write(String.IsNullOrEmpty(message) ? "Working..." : message);
        Console.Write("000.0%");
    }
    public static void ShowProgress(float progress)
    {
        Console.Write($"\b\b\b\b\b\b{progress:000.0%}");
    }
    public static void EndProgress(string? message = null)
    {
        Console.Write("\b\b\b\b\b\b100.0%");
        Console.WriteLine(String.IsNullOrEmpty(message) ? "...done." : message);
    }
}