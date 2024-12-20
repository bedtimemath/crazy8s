using Microsoft.Extensions.Logging;

namespace C8S.Domain.Extensions;

public static class LoggerEx
{
    public static void LogAllLevels(this ILogger logger,
        bool throwException = false)
    {
        logger.LogTrace("Trace");
        logger.LogDebug("Debug");
        logger.LogInformation("Information");
        logger.LogWarning("Warning");
        logger.LogError("Error");
        logger.LogCritical("Critical");

        if (throwException)
            throw new Exception("Something bad happened");
    }
}