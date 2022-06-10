using Microsoft.UI;
using Windows.UI;

namespace IcyLauncher.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly ThemeManager themeManager;
    readonly WindowHandler windowHandler;

    public HomeViewModel(ILogger<HomeViewModel> logger, ThemeManager themeManager, WindowHandler windowHandler)
    {
        this.logger = logger;
        this.themeManager = themeManager;
        this.windowHandler = windowHandler;
    }

    [ICommand]
    void ReColor()
    {
        themeManager.Colors.Accent.Primary = GetRandomColor();
        themeManager.Colors.Accent.Light = GetRandomColor();
        themeManager.Colors.Accent.Dark = GetRandomColor();

        themeManager.Colors.Background.Solid = GetRandomColor();
        themeManager.Colors.Background.Transparent = GetRandomColor(150);

        themeManager.Colors.Control.Primary = GetRandomColor(150);
        themeManager.Colors.Control.Outline = GetRandomColor(150);
        themeManager.Colors.Control.PrimaryDisabled = GetRandomColor(150);
        themeManager.Colors.Control.OutlineDisabled = GetRandomColor(150);

        themeManager.Colors.Control.Solid.Primary = GetRandomColor();
        themeManager.Colors.Control.Solid.Outline = GetRandomColor();
        themeManager.Colors.Control.Solid.PrimaryDisabled = GetRandomColor();
        themeManager.Colors.Control.Solid.OutlineDisabled = GetRandomColor();

        themeManager.Colors.Text.Primary = GetRandomColor();
        themeManager.Colors.Text.Secondary = GetRandomColor();
        themeManager.Colors.Text.Tertiary = GetRandomColor();
        themeManager.Colors.Text.Disabled = GetRandomColor();

        logger.Log("Updated entire fucking theme!!!!");
    }

    static Color GetRandomColor(byte transparency = 255)
    {
        var random = new Random();
        return Color.FromArgb(transparency, Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));
    }


    [ObservableProperty]
    BlurEffect selectedBlurEffect = BlurEffect.Mica;

    partial void OnSelectedBlurEffectChanged(BlurEffect value)
    {
        windowHandler.SetBlur(value, true, true);
    }

    [ObservableProperty]
    IEnumerable<BlurEffect> blurEffects = Enum.GetValues(typeof(BlurEffect)).Cast<BlurEffect>();
}