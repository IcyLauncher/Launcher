using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace IcyLauncher.Helpers.Extentions;

public static class LoggerExtentions
{
    public static void Log(this ILogger logger,
        object? message,
        Exception? exception = null,
        LogLevel logLevel = LogLevel.Information,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "") =>
        logger.Log(exception is null ? logLevel : LogLevel.Error, exception, "[{filePath}-{memberName}] {message}{error}", filePath.Split('\\').Last(), memberName, message, exception.Format());

}