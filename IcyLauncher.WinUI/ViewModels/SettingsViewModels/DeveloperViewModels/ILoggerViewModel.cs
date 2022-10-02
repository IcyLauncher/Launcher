using IcyLauncher.Xaml.Converters;
using Microsoft.UI.Xaml.Data;

namespace IcyLauncher.WinUI.ViewModels;

public partial class ILoggerViewModel : ObservableObject
{
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


    [RelayCommand]
    async Task TestAsync()
    {
        try
        {
            logger.Log(Message_, string.IsNullOrEmpty(Exception) ? null : new(Exception), LogLevel, FilePath, MemberName);
            await message.ShowAsync("logger.Log()", $"Method completed.", closeButton: "Ok");
        }
        catch (Exception ex)
        {
            await message.ShowAsync("logger.Log()", $"Method completed.\nException{ex.Format()}", closeButton: "Ok");
        }
    }


    [RelayCommand]
    void Show()
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