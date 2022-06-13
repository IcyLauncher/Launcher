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
        themeManager.LoadTheme(Theme.Light);
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
        windowHandler.SetBlur(value, true, UseDarkModeBlur);
    }

    [ObservableProperty]
    IEnumerable<BlurEffect> blurEffects = Enum.GetValues(typeof(BlurEffect)).Cast<BlurEffect>();

    [ObservableProperty]
    bool useDarkModeBlur = true;
}