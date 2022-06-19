using IcyLauncher.Core.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;
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

    static Color GetRandomColor(byte transparency = 255)
    {
        var random = new Random();
        return Color.FromArgb(transparency, Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)), Convert.ToByte(random.Next(0, 255)));
    }


    [ObservableProperty]
    BlurEffect selectedBlurEffect = BlurEffect.Mica;

    partial void OnSelectedBlurEffectChanged(BlurEffect value)
    {
        windowHandler.SetBlur(value, true, darkMode);
    }

    [ObservableProperty]
    IEnumerable<BlurEffect> blurEffects = Enum.GetValues(typeof(BlurEffect)).Cast<BlurEffect>();


    [ObservableProperty]
    Color accentPrimary = GetRandomColor(255);

    [ObservableProperty]
    Color accentLight = GetRandomColor(255);

    [ObservableProperty]
    Color accentDark = GetRandomColor(255);

    [ICommand]
    void UpdateAccent()
    {
        //themeManager.Colors.Accent.Primary = accentPrimary;
        //themeManager.Colors.Accent.Light = accentLight;
        //themeManager.Colors.Accent.Dark = accentDark;
        Profiles.Add(new() 
        {
            Title = "Default",
            Color = Colors.Red,
            Icon = "Redstone-Block.png".AsImage(),
            Version = "1.18",
            Client = "Vanilla"
        });
    }

    [ObservableProperty]
    bool darkMode = true;

    partial void OnDarkModeChanged(bool value)
    {
        themeManager.LoadTheme(value ? Theme.Dark : Theme.Light, true);
        windowHandler.SetBlur(selectedBlurEffect, true, darkMode);
    }


    [ObservableProperty]
    ObservableCollection<Profile> profiles = new();
}