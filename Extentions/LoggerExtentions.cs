using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace IcyLauncher.Extentions;

public static class LoggerExtentions
{
    public static void Log(this ILogger logger, object? message, Exception? exception = null, LogLevel logLevel = LogLevel.Information,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "") =>
        logger.Log(logLevel, exception, "[{filePath}-{memberName}] {message}", filePath.Split('\\').Last(), memberName, message);
}