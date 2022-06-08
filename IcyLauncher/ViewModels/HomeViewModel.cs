using Microsoft.UI;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly WindowHandler windowHandler;
    readonly public ThemeManager ThemeManager;

    public HomeViewModel(ILogger<HomeViewModel> logger, WindowHandler windowHandler, ThemeManager themeManager)
    {
        this.logger = logger;
        this.windowHandler = windowHandler;
        ThemeManager = themeManager;
    }

    [ObservableProperty]
    bool enable = true;
    [ObservableProperty]
    bool darkMOde = true;

    [ICommand]
    void Mica()
    {
        windowHandler.SetBlur(BlurEffect.Mica, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void Acrylic()
    {
        windowHandler.SetBlur(BlurEffect.Acrylic, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void Simple()
    {
        windowHandler.SetBlur(BlurEffect.Simple, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }
    [ICommand]
    void None()
    {
        windowHandler.SetBlur(BlurEffect.None, Enable, DarkMOde);
        logger.Log("\n\n\n");
    }

    [ICommand]
    void ReColor()
    {
        ThemeManager.Colors.Accent.Primary = Colors.Red;
        ThemeManager.Colors.Accent.Light = Colors.Green;
        ThemeManager.Colors.Accent.Dark = Colors.Blue;
    }
}