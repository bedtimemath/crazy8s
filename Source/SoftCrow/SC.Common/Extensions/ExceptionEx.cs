using SC.Common.Models;

namespace SC.Common.Extensions;

public static class ExceptionEx
{
    public static SerializableException? ToSerializableException(this Exception? ex) =>
        new SerializableException(ex);

    public static string? ToRecursiveMessage(this Exception? ex) => ex == null ? null : ReadExceptionMessageRecursively(ex);

    private static string? ReadExceptionMessageRecursively(Exception ex) =>
        ex.InnerException == null ? ex.Message + "\r\n****\r\n" + ex.StackTrace
        : ex.Message + "\r\n----\r\n" + ReadExceptionMessageRecursively(ex.InnerException);
}