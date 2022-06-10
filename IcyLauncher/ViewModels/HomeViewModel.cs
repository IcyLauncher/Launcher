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
    }
    [ICommand]
    void Acrylic()
    {
        windowHandler.SetBlur(BlurEffect.Acrylic, Enable, DarkMOde);
    }
    [ICommand]
    void Simple()
    {
        windowHandler.SetBlur(BlurEffect.Simple, Enable, DarkMOde);
    }
    [ICommand]
    void None()
    {
        windowHandler.SetBlur(BlurEffect.None, Enable, DarkMOde);
    }

    [ICommand]
    void ReColor()
    {
        ThemeManager.Colors.Accent.Primary = Colors.Wheat;
        ThemeManager.Colors.Accent.Light = Colors.White;
        ThemeManager.Colors.Accent.Dark = Colors.Black;

        ThemeManager.Colors.Background.Solid = Colors.YellowGreen;
        ThemeManager.Colors.Background.Transparent = Colors.Bisque;

        ThemeManager.Colors.Control.Primary = Colors.Red;
        ThemeManager.Colors.Control.Outline = Colors.Green;
        ThemeManager.Colors.Control.PrimaryDisabled = Colors.Blue;
        ThemeManager.Colors.Control.OutlineDisabled = Colors.Yellow;

        ThemeManager.Colors.Text.Primary = Colors.Violet;
        ThemeManager.Colors.Text.Secondary = Colors.Magenta;
        ThemeManager.Colors.Text.Tertiary = Colors.Turquoise;
        ThemeManager.Colors.Text.Disabled = Colors.Orange;

        logger.Log("Updated entire fucking theme!!!!");
    }
}