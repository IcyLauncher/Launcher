using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IcyLauncher.WinUI.ViewModels;

public partial class DeveloperSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    string logger_message = "test message";

    [ObservableProperty]
    string? logger_exception = null;

    [ObservableProperty]
    LogLevel logger_logLevel = LogLevel.Information;

    [ObservableProperty]
    string logger_filePath = "TestViewModel.cs";

    [ObservableProperty]
    string logger_memberName = "TestMethod";


    [RelayCommand]
    void Logger_Test() =>
        logger.Log(Logger_message, string.IsNullOrEmpty(Logger_exception) ? null : new(Logger_exception), Logger_logLevel, Logger_filePath, Logger_memberName);


    [RelayCommand]
    void Logger_Hook()
    {
    }
}