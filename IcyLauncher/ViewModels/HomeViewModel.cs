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
    bool darkMOde = false;

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
        ThemeManager.Colors.Control.Primary = Colors.Red;
        ThemeManager.Colors.Control.Outline = Colors.Green;
        ThemeManager.Colors.Control.PrimaryDisabled = Colors.Blue;
        ThemeManager.Colors.Control.OutlineDisabled = Colors.Yellow;

        ThemeManager.Colors.Text.Primary = Colors.Violet;
        ThemeManager.Colors.Text.Secondary = Colors.Magenta;
        ThemeManager.Colors.Text.Tertiary = Colors.AliceBlue;
        ThemeManager.Colors.Text.Disabled = Colors.Orange;
    }
}