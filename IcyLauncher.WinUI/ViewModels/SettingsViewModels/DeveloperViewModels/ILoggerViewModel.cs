using IcyLauncher.Xaml.UI;

namespace IcyLauncher.WinUI.ViewModels.SettingsViewModels.DeveloperViewModels;

public partial class ILoggerViewModel : ObservableObject
{
    #region Setup
    readonly ILogger logger;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;
    readonly IMessage message;

    public ILoggerViewModel(
        ILogger logger,
        ThemeManager themeManager,
        WindowHandler windowHandler,
        IMessage message)
    {
        this.logger = logger;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
        this.message = message;
    }
    #endregion


    #region Example
    [ObservableProperty]
    string message_ = "test message";

    [ObservableProperty]
    string? exception = null;

    [ObservableProperty]
    LogLevel logLevel = LogLevel.Information;

    [ObservableProperty]
    string filePath = "TestViewModel.cs";

    [ObservableProperty]
    string memberName = "TestMethod";
    #endregion


    #region Log
    [RelayCommand]
    async Task LogAsync()
    {
        try
        {
            logger.Log(Message_, string.IsNullOrEmpty(Exception) ? null : new(Exception), LogLevel, FilePath, MemberName);
            await message.ShowAsync("logger.Log()", $"Method completed.");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("logger.Log()", $"Method completed.\nException{ex.Format()}");
        }
    }
    #endregion


    #region Show Window
    [RelayCommand]
    void Show()
    {
        if (windowHandler.LoggerWindow is not null)
        {
            windowHandler.LoggerWindow.Activate();
            return;
        }

        LoggerWindow loggerWindow = new() { Title = "IcyLauncher - Logger" };

        void handler(object? s, string e) =>
            loggerWindow.ContentBlock.Text += e;

        App.Sink.OnNewLog += handler;
        loggerWindow.Closed += (s, e) =>
        {
            App.Sink.OnNewLog -= handler;
            windowHandler.LoggerWindow = null;
        };

        windowHandler.SetSize(loggerWindow, 700, 400);
        windowHandler.LoggerWindow = loggerWindow;
        loggerWindow.Activate();

        logger.Log("Created new LoggerWindow and hooked handler");
    }
    #endregion
}