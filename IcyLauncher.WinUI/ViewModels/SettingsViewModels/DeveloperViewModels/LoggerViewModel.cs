using IcyLauncher.Xaml.Converters;
using Microsoft.UI.Xaml.Data;

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
    async Task Logger_Test()
    {
        try
        {
            logger.Log(Logger_message, string.IsNullOrEmpty(Logger_exception) ? null : new(Logger_exception), Logger_logLevel, Logger_filePath, Logger_memberName);
            await message.ShowAsync("logger.Log()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("logger.Log()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    void Logger_Show()
    {
        if (windowHandler.LoggerWindow is not null)
        {
            windowHandler.LoggerWindow.Activate();
            return;
        }

        Window loggerWindow = UIElementProvider.LoggerWindow(out TextBlock content, out ScrollViewer container);

        ColorBrushConverter colorBrushConverter = new();
        container.SetBinding(Control.BackgroundProperty, new Binding()
        {
            Source = themeManager.Colors,
            Converter = colorBrushConverter,
            Path = new PropertyPath("Background.Solid"),
            Mode = BindingMode.OneWay
        });
        content.SetBinding(TextBlock.ForegroundProperty, new Binding()
        {
            Source = themeManager.Colors,
            Converter = colorBrushConverter,
            Path = new PropertyPath("Text.Secondary"),
            Mode = BindingMode.OneWay
        });

        void handler(object? s, string e) => content.Text += e;

        App.Sink.OnNewLog += handler;
        loggerWindow.Closed += (s, e) =>
        {
            App.Sink.OnNewLog -= handler;
            windowHandler.LoggerWindow = null;
        };

        windowHandler.SetSize(loggerWindow, 700, 400);
        windowHandler.LoggerWindow = loggerWindow;
        loggerWindow.Activate();

        logger.Log("Created new window and hooked all logger events");
    }
}