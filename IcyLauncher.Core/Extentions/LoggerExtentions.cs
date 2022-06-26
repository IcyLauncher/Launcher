using System.Runtime.CompilerServices;

namespace IcyLauncher.Core.Extentions;

public static class LoggerExtentions
{
    public static void Log(this ILogger logger, object? message, Exception? exception = null, LogLevel logLevel = LogLevel.Information,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "") =>
        logger.Log(exception is null ? logLevel : LogLevel.Error, exception, "[{filePath}-{memberName}] {message}", filePath.Split('\\').Last(), memberName, message);
}